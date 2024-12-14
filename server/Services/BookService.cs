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
}
