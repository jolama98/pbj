namespace pbj.Models;

public class Comment : RepoItem<int>
{
    public string Body { get; set; }
    public int PoemId { get; set; }
    public string CreatorId { get; set; }
    public Profile Creator { get; set; }
}
