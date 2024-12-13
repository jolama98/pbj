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
}

