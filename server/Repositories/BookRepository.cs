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
        books(title, isPrivate , creatorId)
        VALUES(@title, @isPrivate, @creatorId);

        SELECT
        books.*,
        accounts.*
        FROM books
        JOIN accounts ON accounts.id = books.creatorId
        WHERE books.id = LAST_INSERT_ID();";

        Book book = _db.Query<Book, Profile, Book>(sql, JoinCreator, bookData).FirstOrDefault();
        return book;
    }

    internal void DestroyBook(int bookId)
    {
        string sql = @"
        DELETE FROM books WHERE id = @bookId LIMIT 1;
        ;";
        int rowsAffected = _db.Execute(sql, new { bookId });
        if (rowsAffected == 0) throw new Exception("DELETE FAILED");
        if (rowsAffected > 1) throw new Exception("DELETE WAS OVER POWERED!!!!!!!");
    }

    internal Book GetBookById(int bookId)
    {
        string sql = @"
        SELECT
        books.*,
        accounts.*
        FROM books
        JOIN accounts ON accounts.id = books.creatorId
        WHERE books.id = @bookId;";

        Book book = _db.Query<Book, Profile, Book>(sql, JoinCreator, new
        {
            bookId
        }).FirstOrDefault();
        return book;
    }

    internal List<Book> GetBooksByProfileId(string profileId)
    {
        string sql = @"
        SELECT
        books.*,
        accounts.*
        FROM books
        JOIN accounts ON accounts.id = books.creatorId
        WHERE books.IsPrivate = false AND accounts.id = @profileId;";

        List<Book> book = _db.Query<Book, Profile,
        Book>(sql, JoinCreator, new
        {
            profileId
        }).ToList();
        return book;
    }


    private Book JoinCreator(Book book, Profile profile)
    {
        book.Creator = profile;
        return book;
    }
}

