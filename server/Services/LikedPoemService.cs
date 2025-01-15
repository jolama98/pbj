

namespace pbj.Services;

public class LikedPoemService
{
    private readonly LikedPoemRepository _likedPoemRepository;
    private readonly PoemService _poemService;
    private readonly AccountService _accountService;

    public LikedPoemService(PoemService poemService, LikedPoemRepository likedPoemRepository, AccountService accountService)
    {
        _poemService = poemService;
        _likedPoemRepository = likedPoemRepository;
        _accountService = accountService;
    }

    internal LikedPoem CreateLikedPoem(LikedPoem likedPoemData, string userId)
    {
        Poem poem = _poemService.GetPoemById(likedPoemData.PoemId, userId);
        poem.Likes += 1;

        LikedPoem likedPoem = _likedPoemRepository.CreateLikedPoem(likedPoemData);
        return likedPoem;
    }
}
