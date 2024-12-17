
namespace pbj.Repositories;

public class CommentRepository
{
    private readonly IDbConnection _db;

    public CommentRepository(IDbConnection db)
    {
        _db = db;
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


    private Comment JoinCreator(Comment comment, Profile profile)
    {
        comment.Creator = profile;
        return comment;
    }
}

