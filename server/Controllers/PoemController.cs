namespace pbj.Controllers;

public class PoemController : ControllerBase
{
    private readonly PoemService _poemService;
    private readonly Auth0Provider _auth0Provider;

    public PoemController(PoemService poemService, Auth0Provider auth0Provider)
    {
        _poemService = poemService;
        _auth0Provider = auth0Provider;
    }
}
