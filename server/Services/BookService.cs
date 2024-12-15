
namespace pbj.Services;

public class BookService
{
    private readonly BookRepository _bookRepository;
    private readonly AccountService _accountService;

    public BookService(BookRepository bookRepository, AccountService accountService)
    {
        _bookRepository = bookRepository;
        _accountService = accountService;
    }

    internal Book CreateBook(Book bookData)
    {
        Book book = _bookRepository.CreateBook(bookData);
        return book;
    }

    private Book GetBookById(int bookId)
    {
        Book book = _bookRepository.GetBookById(bookId) ?? throw new Exception($"No book was found with the id of {bookId}");
        return book;
    }
    internal Book GetBookById(int bookId, string userId)
    {
        Book book = GetBookById(bookId);

        if (book.CreatorId != userId)
        {
            throw new Exception("NOTHING TO SEE HERE 👀👀👀");
        }
        return book;
    }

    internal string DestroyBook(int bookId, string userId)
    {
        Book destroyBook = GetBookById(bookId);
        if (destroyBook.CreatorId != userId)
        {
            throw new Exception("YOU DID NOT CREATE THIS BOOK GET AWAY!!!");
        }
        _bookRepository.DestroyBook(bookId);
        return $"{destroyBook.Title} was DELETE";
    }
}
