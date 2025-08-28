

namespace pbj.Services;

public class LikedPoemService
{
    private readonly LikedPoemRepository _likedPoemRepository;
    private readonly PoemRepository _poemRepository;

    private readonly PoemService _poemService;
    private readonly AccountService _accountService;

    // public LikedPoemService(PoemService poemService, LikedPoemRepository likedPoemRepository, AccountService accountService, PoemRepository poemRepository)
    // {
    //     _poemService = poemService;
    //     _likedPoemRepository = likedPoemRepository;
    //     _accountService = accountService;
    //     _poemRepository = poemRepository;
    // }

    // internal LikedPoem CreateLikedPoem(LikedPoem likedPoemData, string userId)
    // {
    //     Poem poem = _poemService.GetPoemById(likedPoemData.PoemId, userId);
    //     poem.Likes += 1;
    //     _poemRepository.UpdatePoem(poem);

    //     LikedPoem likedPoem = _likedPoemRepository.CreateLikedPoem(likedPoemData);
    //     return likedPoem;
    // }
}
