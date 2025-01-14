

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

    internal List<LikedPoem> GetLikedPoemByProfileId(string profileId)
    {
        string sql = @"
        SELECT
        likedPoem.*,
        accounts.*
        FROM
        likedPoem
        JOIN accounts ON accounts.id = likedPoem.creatorId
        WHERE
        accounts.id = @profileId
        GROUP BY likedPoem.id;";

        List<LikedPoem> likedPoems = _db.Query<LikedPoem, Profile, LikedPoem>(sql, (likedPoem, profile) =>
        {
            likedPoem.CreatorId = profile.Id;
            likedPoem.Creator = profile;
            return likedPoem;
        }, new { profileId }).ToList();
        return likedPoems;
    }
}

