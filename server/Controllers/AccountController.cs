namespace pbj.Controllers;


[Authorize]
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
  private readonly LikedPoemService _likedPoemService;
  private readonly AccountService _accountService;
  private readonly Auth0Provider _auth0Provider;

  private readonly BookService _bookService;
  private readonly SavedPoemService _savedPoemService;

  public AccountController(AccountService accountService, Auth0Provider auth0Provider, BookService bookService, LikedPoemService likedPoemService, SavedPoemService savedPoemService)
  {
    _accountService = accountService;
    _auth0Provider = auth0Provider;
    _likedPoemService = likedPoemService;
    _savedPoemService = savedPoemService;
    _bookService = bookService;
  }

  [HttpGet]
  public async Task<ActionResult<Account>> Get()
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      return Ok(_accountService.GetOrCreateAccount(userInfo));
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  [HttpPut]
  public async Task<ActionResult<Account>> EditAccount([FromBody] Account editData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      Account account = _accountService.Edit(editData, accountId: userInfo.Id);
      return Ok(account);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }



  [HttpGet("book")]
  public async Task<ActionResult<List<Book>>> GetBookByAccount()
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      List<Book> book = _accountService.GetBookByAccount(userInfo?.Id);
      return Ok(book);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [HttpGet("savedPoem")]
  async public Task<ActionResult<List<SavedPoem>>> GetAllSavedPoems(string profileId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      List<SavedPoem> savedPoems = _accountService.GetAllSavedPoems(userInfo?.Id);
      return Ok(savedPoems);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [HttpGet("likedPoem")]
  async public Task<ActionResult<List<LikedPoem>>> GetLikedPoems(string profileId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      List<LikedPoem> likedPoems = _accountService.GetLikedPoemByProfileId(userInfo?.Id);
      return Ok(likedPoems);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }
}
