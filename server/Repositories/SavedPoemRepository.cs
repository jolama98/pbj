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
        INSERT INTO
        savedPoem( poemId, bookId, creatorId)
        VALUES(@poemId, @bookId, @creatorId)

        SELECT
        *
        FROM savedPoem
        WHERE savedPoem.id = LAST_INSERT_ID();";

        SavedPoem savedPoem = _db.Query<SavedPoem>(sql, savedPoemData).FirstOrDefault();
        return savedPoem;
    }

    internal void DestroySavePoem(int savedPoemId)
    {
        string sql = "DELETE FROM savedPoem WHERE id = @savedPoemId LIMIT 1;";
        int rowsAffected = _db.Execute(sql, new { savedPoemId });

        if (rowsAffected == 0)
        {
            throw new Exception("DELETE FAILED. CHECK YOUR SQL MANUAL AND YOUR SQL SYNTAX FOR THE ERROR");
        }
        if (rowsAffected > 1)
        {
            throw new Exception("DELETED MORE THAN ONE ROW. CHECK YOUR SQL MANUAL AND YOUR SQL SYNTAX FOR THE ERROR");
        }
    }

    internal SavedPoem GetBookPoemPoemById(int savedPoemId)
    {
        string sql = "SELECT * FROM savedPoem WHERE id = @savedPoem";
        SavedPoem savedPoem = _db.Query<SavedPoem>(sql, new { savedPoemId }).FirstOrDefault();
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

