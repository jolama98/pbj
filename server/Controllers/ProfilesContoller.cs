namespace pbj.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly Auth0Provider _auth0Provider;
    private readonly PoemService _poemService;
    private readonly BookService _bookService;
    private readonly ProfilesService _profilesService;

    public ProfilesController(Auth0Provider auth0Provider, PoemService poemService, ProfilesService profilesService, BookService bookService, LikedPoemService likedPoemService)
    {
        _auth0Provider = auth0Provider;
        _poemService = poemService;
        _profilesService = profilesService;
        _bookService = bookService;

    }

    [HttpGet("{profileId}")]
    public ActionResult<Profile> GetProfileById(string profileId)
    {
        try
        {
            Profile profile = _profilesService.GetProfileById(profileId);
            return Ok(profile);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
    [HttpGet("{profileId}/poem")]
    public ActionResult<List<Poem>> GetPoemsByProfileId(string profileId)
    {
        try
        {
            List<Poem> poem = _poemService.GetPoemByProfileId(profileId);
            return Ok(poem);

        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("{profileId}/books")]
    async public Task<ActionResult<List<Book>>> GetBooksByProfileId(string profileId)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            List<Book> books = _bookService.GetBooksByProfileId(profileId, userInfo?.Id);
            return Ok(books);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }

    }
}