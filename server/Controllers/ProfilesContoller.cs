namespace pbj.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly Auth0Provider _auth0Provider;
    private readonly PoemService _poemService;
    private readonly ProfilesService _profilesService;

    public ProfilesController(Auth0Provider auth0Provider, PoemService poemService, ProfilesService profilesService)
    {
        _auth0Provider = auth0Provider;
        _poemService = poemService;
        _profilesService = profilesService;
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
}