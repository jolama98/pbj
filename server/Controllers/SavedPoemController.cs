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



}
