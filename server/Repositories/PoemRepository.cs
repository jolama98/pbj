namespace pbj.Repositories;

public class PoemRepository
{
    private readonly IDbCommand _db;

    public PoemRepository(IDbCommand db)
    {
        _db = db;
    }
}

