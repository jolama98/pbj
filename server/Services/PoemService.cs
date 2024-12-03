namespace pbj.Services;

public class PoemService
{
    private readonly PoemRepository _poemRepository;

    public PoemService(PoemRepository poemRepository)
    {
        _poemRepository = poemRepository;
    }
}
