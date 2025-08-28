using System.Net.Http;

namespace pbj.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LikedPoemController : ControllerBase
{
    // private readonly Auth0Provider _auth0Provider;

    // private readonly LikedPoemService _likedPoemService;
    // public LikedPoemController(Auth0Provider auth0Provider, LikedPoemService likedPoemService)
    // {
    //     _auth0Provider = auth0Provider;
    //     _likedPoemService = likedPoemService;
    // }
    // [Authorize]
    // [HttpPost]
    // public async Task<ActionResult<LikedPoem>> CreateLikedPoem([FromBody] LikedPoem likedPoemData)
    // {
    //     try
    //     {
    //         Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
    //         likedPoemData.CreatorId = userInfo.Id;
    //         LikedPoem likedPoem = _likedPoemService.CreateLikedPoem(likedPoemData, userInfo.Id);
    //         return Ok(likedPoem);
    //     }
    //     catch (Exception exception)
    //     {
    //         return BadRequest(exception.Message);
    //     }
    // }
}
