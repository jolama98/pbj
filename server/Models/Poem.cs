namespace pbj.Models;
public class Poem : RepoItem<int>
{
    public string Title { get; set; }
    public string Body { get; set; }
    public int Views { get; set; }
    public string Tags { get; set; }
    public int Likes { get; set; }
    public int Saves { get; set; }
    public bool IsArchived { get; set; }
    public string AuthorId { get; set; }
    public string Image { get; set; }
    public Profile Creator { get; set; }
}
