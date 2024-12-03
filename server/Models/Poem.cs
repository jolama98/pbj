namespace pbj.Models;
public class Poem : RepoItem<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Views { get; set; }
    public string CreatorId { get; set; }
    public Profile Creator { get; set; }
}
