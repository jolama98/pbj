
namespace pbj.Models;
public class SavedPoem : RepoItem<int>
{
    // RELATIONSHIP PROPERTIES
    public int PoemId { get; set; }
    public int BookId { get; set; }
    public string CreatorId { get; set; }
}
public class SavedPoemPoem : Poem
{
    public int SavedPoemId { get; set; }
    public int BookId { get; set; }
    public string AccountId { get; set; }
}