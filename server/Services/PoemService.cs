namespace pbj.Services;
public class PoemService
{
    private readonly PoemRepository _poemRepository;
    private readonly CommentService _commentService;

    public PoemService(PoemRepository poemRepository, CommentService commentService)
    {
        _poemRepository = poemRepository;
        _commentService = commentService;
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

    internal string DestroyPoem(int poemId, string userId)
    {
        Poem poem = GetPoemById(poemId);
        if (poem.AuthorId != userId)
        {
            throw new Exception("YOU DID NOT CREATE THIS POEM! GET AWAY!!!!!");
        }
        _poemRepository.DestroyPoem(poemId);
        return "Poem was DELETED!";
    }


    // internal Poem GetAllCommentsByPoemId(int poemId, int commentId)
    // {
    //     Comment comment = _commentService.GetCommentById(commentId);
    //     Poem poem = _poemRepository.GetPoemById(poemId) ?? throw new Exception($"No Poem found with this id of {poemId}");

    //     return poem;
    // }

    internal Poem GetPoemById(int poemId)
    {
        Poem poem = _poemRepository.GetPoemById(poemId) ?? throw new Exception($"No Poem found with this id of {poemId}");
        poem.Views += 1;
        _poemRepository.UpdatePoem(poem);
        return poem;
    }

    internal Poem GetPoemById(int poemId, string userId)
    {
        Poem poem = GetPoemById(poemId);
        return poem;
    }

    internal Poem UpdatePoem(int poemId, string userId, Poem poemData)
    {
        Poem poemToUpdate = GetPoemById(poemId);
        if (poemToUpdate.AuthorId != userId)
        {
            throw new Exception("YOU CANNOT UPDATE A POEM YOU DID NOT CREATE, THAT IS FORBIDDEN, PLEASE IGNORE THE 400 ERROR CODE, IT SHOULD BE 403");
        }
        poemToUpdate.Title = poemData.Title ?? poemData.Title;
        poemToUpdate.Body = poemData.Body ?? poemData.Body;
        _poemRepository.UpdatePoem(poemToUpdate);

        return poemToUpdate;
    }

    internal List<Poem> GetPoemByProfileId(string profileId)
    {
        List<Poem> poem = _poemRepository.GetPoemByProfileId(profileId);
        return poem;
    }

    internal List<Poem> SearchForPoems(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            throw new ArgumentException("Search term cannot be empty or whitespace.");
        }

        return _poemRepository.SearchPoems(keyword);
    }
}
