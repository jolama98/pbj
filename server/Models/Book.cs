namespace pbj.Models;

public class Book : RepoItem<int>
{
    public string Title { get; set; }
    public string CreatorId { get; set; }
    public Profile Creator { get; set; }
}
