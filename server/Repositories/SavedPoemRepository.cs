
namespace pbj.Repositories;

public class SavedPoemRepository
{
    private readonly IDbConnection _db;

    public SavedPoemRepository(IDbConnection db)
    {
        _db = db;
    }

    internal SavedPoem CreateSavedPoem(SavedPoem savedPoemData)
    {
        string sql = @"
        INERT INTO
        savedPoem(poemId, creatorId)
        VALUES(@poemId, @creatorId)

        SELECT
        *
        FROM savePoem
        WHERE savePoem.id = LAST_INSERT_ID();";

        SavedPoem savedPoem = _db.Query<SavedPoem>(sql, savedPoemData).FirstOrDefault();
        return savedPoem;
    }

    internal List<SavedPoemPoem> GetPoemsInBook(int bookId, string userId)
    {
        string sql = @"
        SELECT
        savedPoem.*,
        poem.*,
        accounts.*
        FROM savedPoem
        JOIN accounts ON accounts.id = savedPoem.creatorId
        JOIN poem ON poem.id = savedPoem.poemId
        WHERE savedPoem.bookId = @bookId
        ;";

        List<SavedPoemPoem> savedPoemPoem = _db.Query<SavedPoem, SavedPoemPoem, Profile, SavedPoemPoem>(sql, (savedPoem, poem, profile) =>
        {
            poem.AccountId = savedPoem.CreatorId;
            poem.SavedPoemId = savedPoem.Id;
            poem.Creator = profile;
            return poem;
        }, new { userId, bookId }).ToList();
        return savedPoemPoem;
    }
}

