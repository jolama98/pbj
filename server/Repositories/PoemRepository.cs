namespace pbj.Repositories;

public class PoemRepository
{
    private readonly IDbConnection _db;

    public PoemRepository(IDbConnection db)
    {
        _db = db;
    }
    internal List<Poem> GetAllPoems()
    {
        string sql = @"
        poem.*,
        accounts.*
        FORM poem
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
}

