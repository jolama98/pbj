
namespace pbj.Controllers;

/*
===============================================================================
PoemController — HTTP API for Poems
-------------------------------------------------------------------------------
CHEAT SHEET (read this first when you come back later):

- ROUTE BASE: /api/poem
- AUTH:
  - Anonymous can GET and SEARCH.
  - Write ops (POST/PUT/DELETE) require [Authorize] and use Auth0 user id.
- STATUS CODES:
  - 200 OK: successful read/update
  - 201 Created: after creating a poem (returns Location header)
  - 204 NoContent: after deleting a poem
  - 400 BadRequest: validation/runtime errors (bubble service exceptions)
- PAGINATION: GET /api/poem?skip=0&take=50 (take capped to 100)
- FILTERS: optional authorId, tag, genre on list endpoint
- FULLTEXT SEARCH: /api/poem/search?query=... (backed by FULLTEXT(title, body))
- PER-USER FIELDS:
  - IsLiked is NOT stored on the poem row. The service can compute it using
    likedPoem for the current user (if available).

SERVICE CONTRACT EXPECTATIONS (PoemService):
  - List: GetAllPoems(int skip, int take, string? authorId, string? tag, string? genre)
  - Read: GetPoemById(int poemId, string? currentUserId)  // currentUserId optional
  - Search: SearchForPoems(string query, int skip, int take)
  - Create: CreatePoem(Poem poemData)  // sets Id, timestamps, etc.
  - Update: UpdatePoem(int poemId, string userId, Poem poemData)  // author-only
  - Delete: DestroyPoem(int poemId, string userId)  // author-only

SECURITY NOTES:
  - AuthorId is taken from the authenticated user (never trust client input).
  - On Update/Delete, service must verify userId == poem.AuthorId (or admin).
===============================================================================
*/

[ApiController]
[Route("api/[controller]")]
public class PoemController : ControllerBase
{
    private readonly PoemService _poemService;        // business logic / data access
    private readonly Auth0Provider _auth0Provider;     // gets current user (Auth0)
    

    public PoemController(PoemService poemService, Auth0Provider auth0Provider)
    {
        _poemService = poemService;
        _auth0Provider = auth0Provider;

    }

    // =========================================================================
    // GET /api/poem
    // Lists poems with optional pagination and filters.
    // - skip/take: simple paging (take is capped to 100)
    // - authorId: filter by author
    // - tag: filter where tags CSV contains this value (service implements)
    // - genre: filter via poemGenre join (service implements)
    // =========================================================================
    [HttpGet]
    public ActionResult<List<Poem>> GetAllPoems(
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50,
        [FromQuery] string? authorId = null,
        [FromQuery] string? tag = null,
        [FromQuery] string? genre = null)
    {
        try
        {
            take = Math.Clamp(take, 1, 100); // safety cap to prevent huge pulls
            var poems = _poemService.GetAllPoems(skip, take, authorId, tag, genre);
            return Ok(poems);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // =========================================================================
    // GET /api/poem/{poemId}
    // Fetch a single poem.
    // - We *optionally* pull the current user to allow service to compute IsLiked.
    // - If user is anonymous, currentUserId will be null (service should handle).
    // =========================================================================
    [HttpGet("{poemId:int}")]
    public async Task<ActionResult<Poem>> GetPoemById(int poemId)
    {
        try
        {
            string? currentUserId = null;

            // Best-effort: If the caller is authenticated, include their id.
            // If not authenticated, this throws; we ignore it and continue as anon.
            try
            {
                var user = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
                currentUserId = user?.Id;
            }
            catch { /* ignore unauthenticated */ }

            var poem = _poemService.GetPoemById(poemId, currentUserId);
            return Ok(poem);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // =========================================================================
    // GET /api/poem/search?query=...&skip=0&take=50
    // Full-text search over title/body (requires FULLTEXT index on poem table).
    // - The service should use MATCH(title, body) AGAINST (@query IN NATURAL LANGUAGE MODE)
    // - Returns 400 if query is empty to avoid scanning/accidental heavy queries.
    // =========================================================================
    [HttpGet("search")]
    public ActionResult<List<Poem>> SearchPoems([FromQuery] string query, [FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Search query cannot be empty.");

            take = Math.Clamp(take, 1, 100);

            var poems = _poemService.SearchForPoems(query, skip, take);
            return Ok(poems);
        }
        catch (Exception ex)
        {
       
            return BadRequest(ex.Message);
        }
    }

    // =========================================================================
    // POST /api/poem   [Authorize]
    // Create a new poem.
    // - AuthorId is always set from the authenticated user (ignore client value).
    // - Returns 201 Created with Location header pointing to GET /api/poem/{id}.
    // =========================================================================
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Poem>> CreatePoem([FromBody] Poem poemData)
    {
        try
        {
            var userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            poemData.AuthorId = userInfo.Id;

            var created = _poemService.CreatePoem(poemData);
            return CreatedAtAction(nameof(GetPoemById), new { poemId = created.Id }, created);
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

    // =========================================================================
    // PUT /api/poem/{poemId}   [Authorize]
    // Update an existing poem.
    // - Service must verify the caller owns the poem (poem.AuthorId == user.Id)
    // - Returns the updated poem.
    // =========================================================================
    [Authorize]
    [HttpPut("{poemId:int}")]
    public async Task<ActionResult<Poem>> UpdatePoem(int poemId, [FromBody] Poem poemData)
    {
        try
        {
            var userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            var updated = _poemService.UpdatePoem(poemId, userInfo.Id, poemData);
            return Ok(updated);
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

    // =========================================================================
    // DELETE /api/poem/{poemId}   [Authorize]
    // Delete a poem.
    // - Service must verify the caller owns the poem (or has admin permission).
    // - Returns 204 NoContent (no body) on success.
    // =========================================================================
    [Authorize]
    [HttpDelete("{poemId:int}")]
    public async Task<IActionResult> DestroyPoem(int poemId)
    {
        try
        {
            var userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            _poemService.DestroyPoem(poemId, userInfo.Id);
            return NoContent(); // preferred for delete
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

    // =========================================================================
    // (OPTIONAL) GET /api/poem/{poemId}/comments
    // When comments service is ready, expose this:
    //
    // [HttpGet("{poemId:int}/comments")]
    // public ActionResult<List<Comment>> GetCommentsForPoem(int poemId)
    // {
    //     try
    //     {
    //         var comments = _poemService.GetAllCommentsByPoemId(poemId);
    //         return Ok(comments);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "GetCommentsForPoem failed (poemId={PoemId})", poemId);
    //         return BadRequest(ex.Message);
    //     }
    // }
    // =========================================================================
}
