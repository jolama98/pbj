
namespace pbj.Models;
public class SavedPoem : RepoItem<int>
{
    // public int Id { get; set; }
    // public DateTime CreatedAt { get; set; }
    // public DateTime UpdatedAt { get; set; }

    // RELATIONSHIP PROPERTIES
    public string CreatorId { get; set; }
    public int PoemId { get; set; }
    public int BookId { get; set; }
}
public class SavedPoemPoem : Poem
{
    public int SavedPoemId { get; set; }
    public int BookId { get; set; }
    public string AccountId { get; set; }
}