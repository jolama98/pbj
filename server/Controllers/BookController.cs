namespace pbj.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly BookService _bookService;
    private readonly Auth0Provider _auth0Provider;

    public BookController(BookService bookService, Auth0Provider auth0Provider)
    {
        _bookService = bookService;
        _auth0Provider = auth0Provider;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook([FromBody] Book bookData)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            bookData.CreatorId = userInfo.Id;
            Book book = _bookService.CreateBook(bookData);
            return Ok(book);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
