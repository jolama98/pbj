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
        poem(title, body, tags, isArchived,  authorId)
        VALUES(@title, @body, @tags, @isArchived,  @authorId);

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

    private Poem JoinCreator(Poem poem, Profile profile)
    {
        poem.Creator = profile;
        return poem;
    }
}

