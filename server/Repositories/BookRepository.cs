

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

    internal void DestroyBook(int bookId)
    {
        string sql = @"
         DELETE FROM book WHERE id = @bookId LIMIT 1;
        ;";
        int rowsAffected = _db.Execute(sql, new { bookId });
        if (rowsAffected == 0) throw new Exception("DELETE FAILED");
        if (rowsAffected > 1) throw new Exception("DELETE WAS OVER POWERED!!!!!!!");
    }

    internal Book GetBookById(int bookId)
    {
        string sql = @"
        SELECT
        book.*,
        accounts.*
        FROM book
        JOIN accounts ON accounts.id = book.creatorId
        WHERE book.id = @bookId
        ;";

        Book book = _db.Query<Book, Profile, Book>(sql, JoinCreator, new
        {
            bookId
        }).FirstOrDefault();
        return book;
    }

    private Book JoinCreator(Book book, Profile profile)
    {
        book.Creator = profile;
        return book;
    }
}

