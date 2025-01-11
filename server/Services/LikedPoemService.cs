

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
        LikedPoem likedPoem = _likedPoemRepository.CreateLikedPoem(likedPoemData);
        return likedPoem;
    }

    // internal List<LikedPoem> GetLikedPoemByProfileId(string profileId, string userId)
    // {
    //     if (profileId == userId)
    //     {
    //         List<LikedPoem> likedPoem = _accountService.GetLikedPoemByAccount(profileId);
    //         return likedPoem;
    //     }
    //     List<LikedPoem> likedPoems = _likedPoemRepository.GetLikedPoemByProfileId(profileId);
    //     return likedPoems;
    // }
}
