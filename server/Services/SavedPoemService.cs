

namespace pbj.Services;

public class SavedPoemService
{
    private readonly SavedPoemRepository _savedPoemRepository;
    private readonly BookService _bookService;

    public SavedPoemService(SavedPoemRepository savedPoemRepository)
    {
        _savedPoemRepository = savedPoemRepository;
    }

    internal SavedPoem CreateSavedPoem(SavedPoem savedPoemData)
    {
        throw new NotImplementedException();
    }

    internal List<SavedPoemPoem> GetPublicBook(int bookId, string userId)
    {
        Book book = _bookService.GetPublicBook(bookId, userId);
        List<SavedPoemPoem> savedPoem = _savedPoemRepository.GetPoemsInBook(bookId, userId);
        return savedPoem;
    }

}
