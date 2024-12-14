namespace pbj.Repositories;

public class BookRepository
{
    private readonly IDbConnection _db;

    public BookRepository(IDbConnection db)
    {
        _db = db;
    }

    internal Book CreateBook(Book bookData)
    {
        string sql = @"
        INSERT INTO
        books(title, creatorId)
        VALUES(@title, @creatorId);

        SELECT
        books.*,
        accounts.*
        FROM books
        JOIN accounts ON accounts.id = books.creatorId
        WHERE books.id = LAST_INERT_ID();";

        Book book = _db.Query<Book, Profile, Book>(sql, JoinCreator, bookData).FirstOrDefault();
        return book;
    }


    private Book JoinCreator(Book book, Profile profile)
    {
        book.Creator = profile;
        return book;
    }
}

