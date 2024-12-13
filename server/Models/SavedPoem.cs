namespace pbj.Models;

public class SavedPoem : RepoItem<int>
{
    public int PoemId { get; set; }
    public string CreatorId { get; set; }
}

public class SavedPoemProfile : Profile
{
    public string SavedPoemId { get; set; }
    public int PoemId { get; set; }
}
