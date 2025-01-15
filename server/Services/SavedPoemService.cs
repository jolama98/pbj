namespace pbj.Services;


public class SavedPoemService
{
    private readonly SavedPoemRepository _savedPoemRepository;
    private readonly BookService _bookService;

    public SavedPoemService(SavedPoemRepository savedPoemRepository, BookService bookService)
    {
        _savedPoemRepository = savedPoemRepository;
        _bookService = bookService;
    }

    internal SavedPoem CreateSavedPoem(SavedPoem savedPoemData, string userId)
    {
        Book book = _bookService.GetBookById(savedPoemData.BookId, userId);
        SavedPoem savedPoem = _savedPoemRepository.CreateSavedPoem(savedPoemData);
        if (book.CreatorId != userId) throw new Exception("You can't create a savePoem in this book!");

        return savedPoem;

    }

    internal string DestroySavePoem(int savedPoemId, string userId)
    {
        SavedPoem savedPoem = GetBookPoemPoemById(savedPoemId);
        Book book = _bookService.GetBookId(savedPoem.BookId);
        if (savedPoem.CreatorId != userId)
        {
            throw new Exception($"This is not yours to destroy {savedPoem.CreatorId}");

        }
        _savedPoemRepository.DestroySavePoem(savedPoemId);
        return "Save Poem has been dealt with";
    }

    private SavedPoem GetBookPoemPoemById(int savedPoemId)
    {
        SavedPoem savedPoem = _savedPoemRepository.GetBookPoemPoemById(savedPoemId) ?? throw new Exception($"No vault keep found with the id of {savedPoemId}");
        return savedPoem;
    }

    internal List<SavedPoemPoem> GetPublicBook(int bookId, string userId)
    {
        Book book = _bookService.GetPublicBook(bookId, userId);
        List<SavedPoemPoem> savedPoem = _savedPoemRepository.GetPoemsInBook(bookId, userId);
        return savedPoem;
    }

}
