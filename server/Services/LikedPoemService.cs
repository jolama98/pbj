
namespace pbj.Services;

public class LikedPoemService
{
    private readonly LikedPoemRepository _likedPoemRepository;
    private readonly PoemService _poemService;

    public LikedPoemService(PoemService poemService, LikedPoemRepository likedPoemRepository)
    {
        _poemService = poemService;
        _likedPoemRepository = likedPoemRepository;
    }

    internal LikedPoem CreateLikedPoem(LikedPoem likedPoemData, string userId)
    {
        Poem poem = _poemService.GetPoemById(likedPoemData.PoemId, userId);
        LikedPoem likedPoem = _likedPoemRepository.CreateLikedPoem(likedPoemData);
        return likedPoem;
    }
}
