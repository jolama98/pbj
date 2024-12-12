namespace pbj.Services;

public class PoemService
{
    private readonly PoemRepository _poemRepository;

    public PoemService(PoemRepository poemRepository)
    {
        _poemRepository = poemRepository;
    }

    internal List<Poem> GetAllPoems()
    {
        List<Poem> poem = _poemRepository.GetAllPoems();
        return poem;
    }

    internal Poem CreatePoem(Poem poemData)
    {
        Poem poem = _poemRepository.CreatePoem(poemData);
        return poem;
    }
}
