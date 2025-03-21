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
    //SECTION - get all poems
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

    //SECTION - Create a poem
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
    //SECTION Destroy Poem
    [Authorize]
    [HttpDelete("{poemId}")]
    public async Task<ActionResult<string>> DestroyPoem(int poemId)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            string message = _poemService.DestroyPoem(poemId, userInfo.Id);
            return Ok(message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    //SECTION get poem by id
    [HttpGet("{poemId}")]
    public async Task<ActionResult<Poem>> GetPoemById(int poemId)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            Poem poem = _poemService.GetPoemById(poemId);
            return Ok(poem);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }


    //SECTION - search for poem
    [HttpGet("search")]
    public ActionResult<List<Poem>> SearchPoems([FromQuery] string query)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            List<Poem> poems = _poemService.SearchForPoems(query);
            return Ok(poems);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }



    //SECTION - edit poem
    [Authorize]
    [HttpPut("{poemId}")]
    public async Task<ActionResult<Poem>> UpdatePoem(int poemId, [FromBody] Poem poemData)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            Poem poem = _poemService.UpdatePoem(poemId, userInfo.Id, poemData);
            return Ok(poem);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    // //SECTION Get All Comments By PoemId
    // [HttpGet("{poemId}/comments")]
    // public async Task<ActionResult<Poem>> GetAllCommentsByPoemId(int poemId)
    // {
    //     try
    //     {
    //         Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);

    //         Poem poem = _poemService.GetAllCommentsByPoemId(poemId);
    //         return Ok(poem);
    //     }
    //     catch (Exception exception)
    //     {
    //         return BadRequest(exception.Message);
    //     }
    // }
}
