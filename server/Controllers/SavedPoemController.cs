namespace pbj.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SavedPoemController : ControllerBase
{
    private readonly SavedPoemService _savedPoemService;
    private readonly Auth0Provider _auth0Provider;

    public SavedPoemController(SavedPoemService savedPoemService, Auth0Provider auth0Provider)
    {
        _savedPoemService = savedPoemService;
        _auth0Provider = auth0Provider;
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
            SavedPoem savedPoem = _savedPoemService.CreateSavedPoem(savedPoemData);
            return Ok(savedPoem);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

}
