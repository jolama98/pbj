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

    //SECTION - CreateAComment
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Comment>> CreateAComment([FromBody] Comment commentData)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            commentData.CreatorId = userInfo.Id;
            Comment comment = _commentService.CreateAComment(commentData);
            return Ok(comment);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
    //SECTION - Get Comment By Id
    [HttpGet("{commentId}")]
    public async Task<ActionResult<Comment>> GetCommentById(int commentId)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            Comment comment = _commentService.GetCommentById(commentId);
            return comment;
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }


    //SECTION -  Delete Comment
    [Authorize]
    [HttpDelete("{commentId}")]
    public async Task<ActionResult<string>> DestroyComment(int commentId)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            string message = _commentService.DestroyComment(commentId, userInfo.Id);
            return Ok(message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    //SECTION - Edit Comment
    [Authorize]
    [HttpPut("{commentId}")]
    public async Task<ActionResult<Comment>> UpdateComment(int commentId, [FromBody] Comment commentData)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            Comment comment = _commentService.UpdateComment(commentId, userInfo.Id, commentData);
            return Ok(comment);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

}
