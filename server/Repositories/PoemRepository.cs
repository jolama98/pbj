namespace pbj.Repositories;

public class PoemRepository
{
    private readonly IDbConnection _db;

    public PoemRepository(IDbConnection db)
    {
        _db = db;
    }

    internal Poem CreatePoem(Poem poemData)
    {
        string sql = @"
        INSERT INTO
        poem(title, body, tags, isArchived, image,  authorId)
        VALUES(@title, @body, @tags, @isArchived,  @image,@authorId);

        SELECT
        poem.*,
        accounts.*
        FROM poem
        JOIN accounts ON accounts.id = poem.authorId
        WHERE poem.id = LAST_INSERT_ID();";

        Poem poem = _db.Query<Poem, Profile, Poem>(sql, JoinCreator, poemData).FirstOrDefault();
        return poem;
    }

    internal List<Poem> GetAllPoems()
    {
        string sql = @"
        SELECT
        poem.*,
        accounts.*
        FROM poem
        JOIN accounts ON accounts.id = poem.authorId
        GROUP BY (poem.id)
        ;";


        List<Poem> poem = _db.Query<Poem, Profile, Poem>(sql, (poem, profile) =>
        {
            poem.Creator = profile;
            return poem;
        }).ToList();
        return poem;
    }

    internal List<Poem> SearchPoems(string searchTerm)
    {
        string sql = @"
    SELECT
    poem.*,
    accounts.*
    FROM poem
    JOIN accounts ON accounts.id = poem.authorId
    WHERE
        poem.title LIKE CONCAT('%', @searchTerm, '%') OR
        poem.body LIKE CONCAT('%', @searchTerm, '%') OR
        poem.tags LIKE CONCAT('%', @searchTerm, '%')
    GROUP BY poem.id
    ;";

        List<Poem> poems = _db.Query<Poem, Profile, Poem>(sql, (poem, profile) =>
        {
            poem.Creator = profile;
            return poem;
        }, new { searchTerm }).ToList();

        return poems;
    }



    internal void DestroyPoem(int poemId)
    {
        string sql = "DELETE FROM poem WHERE id = @poemId LIMIT 1";
        int rowsAffected = _db.Execute(sql, new
        {
            poemId
        });

        if (rowsAffected == 0) throw new Exception("DELETE FAILED");
        if (rowsAffected > 1) throw new Exception("DELETE WAS OVER POWERED!!!!!!!");
    }


    internal Poem GetPoemById(int poemId)
    {
        string sql = @"
        SELECT 
        poem.*,
        accounts.*
        FROM poem
        JOIN accounts ON accounts.id = poem.authorId
        WHERE poem.id = @poemId
        GROUP BY (poem.id)
        ;";

        Poem poem = _db.Query<Poem, Profile, Poem>(sql, JoinCreator, new
        {
            poemId
        }).FirstOrDefault();

        return poem;
    }

    internal List<Poem> GetPoemByProfileId(string profileId)
    {
        string sql = @"
        SELECT
        *
        FROM poem
        JOIN accounts ON accounts.id = poem.creatorId
        WHERE accounts.id = @profileId
        ;";

        List<Poem> poem = _db.Query<Poem, Profile, Poem>(sql, JoinCreator, new
        {
            profileId
        }).ToList();
        return poem;
    }
    internal void UpdatePoem(Poem poemToUpdate)
    {
        string sql = @"
        UPDATE poem
        Set
        title = @title,
        body = @body,
        views = @views
        WHERE id = @Id LIMIT 1;";

        int rowsAffected = _db.Execute(sql, poemToUpdate);
        if (rowsAffected == 0) throw new Exception("UPDATE FAILED");
        if (rowsAffected > 1) throw new Exception("UPDATE DID NOT FAIL, BUT THAT IS STILL A PROBLEM");

    }
    private Poem JoinCreator(Poem poem, Profile profile)
    {
        poem.Creator = profile;
        return poem;
    }

}

