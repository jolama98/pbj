namespace pbj.Controllers;

[ApiController]
[Route("api/controller")]
public class CommentController : ControllerBase
{
    private readonly Auth0Provider _auth0Provider;
    private readonly CommentService _commentService;

    public CommentController(CommentService commentService, Auth0Provider auth0Provider)
    {
        _commentService = commentService;
        _auth0Provider = auth0Provider;
    }

    //SECTION - GetAllComments
    [HttpGet]
    public ActionResult<List<Comment>> GetAllComments()
    {
        try
        {
            List<Comment> comment = _commentService.GetAllComments();
            return Ok(comment);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

}
