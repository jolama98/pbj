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
        comment(title, body, creatorId)
        VALUES(@title, @body, @creatorId);

        SELECT
        comment.*,
        accounts.*
        FROM comment
        JOIN accounts ON accounts.id = comment.creatorId
        WHERE comment.id = LAST_INSERT_ID();";

        Comment comment = _db.Query<Comment, Profile, Comment>(sql, JoinCreator, commentData).FirstOrDefault();
        return comment;
    }

    internal void DestroyComment(int commentId)
    {
        string sql = "DELETE FROM comment WHERE id = @comment LIMIT 1";
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
        comment.*,
        accounts.*
        FROM comment
        JOIN accounts ON accounts.id = comment.creatorId
        GROUP BY (comment.id)
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
            comment.*
            account.*
            FROM comment
            JOIN accounts ON accounts.id = comment.creatorId
            WHERE comment.id = @commentId
            GROUP BY (comment.id)
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
        UPDATE comment
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

