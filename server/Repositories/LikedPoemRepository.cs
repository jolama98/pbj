
namespace pbj.Repositories;

public class LikedPoemRepository
{
    private readonly IDbConnection _db;

    public LikedPoemRepository(IDbConnection db)
    {
        _db = db;
    }

    internal LikedPoem CreateLikedPoem(LikedPoem likedPoemData)
    {
        string sql = @"
        INSERT INTO
    likedPoem (poemId, creatorId)
    VALUES (@PoemId, @CreatorId);
    SELECT * FROM likedPoem WHERE likedPoem.id = LAST_INSERT_ID();";

        LikedPoem likedPoem = _db.Query<LikedPoem>(sql, likedPoemData).FirstOrDefault();
        return likedPoem;
    }
}

