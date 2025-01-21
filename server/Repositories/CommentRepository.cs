namespace pbj.Repositories;

public class CommentRepository
{
    private readonly IDbConnection _db;

    public CommentRepository(IDbConnection db)
    {
        _db = db;
    }

    internal Comment CreateAComment(Comment commentData)
    {
        string sql = @"
        INSERT INTO
        comments( body, poemId, creatorId)
        VALUES( @body, @poemId, @creatorId);

        SELECT
        *
        FROM comments
         JOIN accounts ON accounts.id = comments.creatorId
        WHERE comments.id = LAST_INSERT_ID();";

        Comment comment = _db.Query<Comment, Profile, Comment>(sql, JoinCreator, commentData).FirstOrDefault();
        return comment;
    }

    internal void DestroyComment(int commentId)
    {
        string sql = "DELETE FROM comments WHERE id = @comment LIMIT 1";
        int rowsAffected = _db.Execute(sql, new
        {
            commentId
        });
        if (rowsAffected == 0) throw new Exception("DELETE FAILED");
        if (rowsAffected > 1) throw new Exception("DELETE WAS OVER POWERED!!!!!!!");
    }

    internal List<Comment> GetAllComments()
    {
        string sql = @"
        SELECT
        comments.*,
        accounts.*
        FROM comments
        JOIN accounts ON accounts.id = comments.creatorId
        GROUP BY (comments.id)
        ;";

        List<Comment> comment = _db.Query<Comment, Profile, Comment>(sql, (comment, profile) =>
        {
            comment.Creator = profile;
            return comment;
        }).ToList();
        return comment;
    }

    internal Comment GetCommentById(int commentId)
    {
        string sql = @"
            SELECT
            comments.*
            account.*
            FROM comments
            JOIN accounts ON accounts.id = comments.creatorId
            WHERE comments.id = @comments
            GROUP BY (comments.id)
        ;";

        Comment comment = _db.Query<Comment, Profile, Comment>(sql, JoinCreator, new
        {
            commentId
        }).FirstOrDefault();

        return comment;
    }

    internal void UpdateComment(Comment commentToUpdate)
    {
        string sql = @"
        UPDATE comments
        Set
        title = @title,
        body = @body,
        WHERE id = @Id LIMIT 1;";

        int rowsAffected = _db.Execute(sql, commentToUpdate);
        if (rowsAffected == 0) throw new Exception("UPDATE FAILED");
        if (rowsAffected > 1) throw new Exception("UPDATE DID NOT FAIL, BUT THAT IS STILL A PROBLEM");
    }

    private Comment JoinCreator(Comment comment, Profile profile)
    {
        comment.Creator = profile;
        return comment;
    }
}

