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


    [Authorize]
    //NOTE - ask jake how to limit creation to one account
    [HttpPost]
    public async Task<ActionResult<Poem>> CreatePoem([FromBody] Poem poemData)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            poemData.AuthorId = userInfo.Id;
            Poem poem = _poemService.CreatePoem(poemData);
            return Ok(poem);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
