namespace pbj.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SavedPoemController : ControllerBase
{
    private readonly SavedPoemService _savedPoemService;
    private readonly Auth0Provider _auth0Provider;

    public SavedPoemController(Auth0Provider auth0Provider, SavedPoemService savedPoemService)
    {
        _auth0Provider = auth0Provider;
        _savedPoemService = savedPoemService;
    }

    //SECTION - Create many to many
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<SavedPoem>> CreateSavedPoem([FromBody] SavedPoem savedPoemData)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);

            savedPoemData.CreatorId = userInfo.Id;
            SavedPoem savedPoem = _savedPoemService.CreateSavedPoem(savedPoemData, userInfo.Id);
            return Ok(savedPoem);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
    [HttpDelete("{savedPoemId}")]
    [Authorize]
    public async Task<ActionResult<string>> DestroySavePoem(int savedPoemId)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            string message = _savedPoemService.DestroySavePoem(savedPoemId, userInfo.Id);
            return Ok(message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

}
