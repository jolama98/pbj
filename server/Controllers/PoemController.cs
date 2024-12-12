namespace pbj.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PoemController : ControllerBase
{
    private readonly PoemService _poemService;
    private readonly Auth0Provider _auth0Provider;

    public PoemController(PoemService poemService, Auth0Provider auth0Provider)
    {
        _poemService = poemService;
        _auth0Provider = auth0Provider;
    }

    [HttpGet]
    public ActionResult<List<Poem>> GetAllPoems()
    {
        try
        {
            List<Poem> poem = _poemService.GetAllPoems();
            return Ok(poem);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
