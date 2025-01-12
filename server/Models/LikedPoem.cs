
namespace pbj.Models;

public class LikedPoem : RepoItem<int>
{
    public int PoemId { get; set; }
    public string CreatorId { get; set; }
    public Profile Creator { get; set; }

}
