namespace pbj.Services;

public class SavedPoemService
{
    private readonly SavedPoemRepository _savedPoemRepository;

    public SavedPoemService(SavedPoemRepository savedPoemRepository)
    {
        _savedPoemRepository = savedPoemRepository;
    }
    // internal SavedPoem CreateSavePoem(SavedPoem savedPoemData)
    // {

    // }
}
