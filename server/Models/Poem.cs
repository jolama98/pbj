namespace pbj.Models;

public class Poem : RepoItem<int>
{
    public string Title { get; set; }
    public string Body { get; set; }
    public int Views { get; set; }
    public string Tags { get; set; }
    public int Likes { get; set; }       // cached like count
    public int Saves { get; set; }       // cached save count

    public bool IsArchived { get; set; } // from DB (tinyint(1), default false)
    public bool IsLiked { get; set; }    // computed per user (not stored in poem table)

    public string AuthorId { get; set; } // FK → accounts.id


    /// <summary>
    /// The Profile object for the author of this poem.
    /// </summary>
    public Profile Creator { get; set; }

    /// <summary>
    /// Genres this poem is tagged with (via poemGenre join table).
    /// </summary>
    public List<string> Genres { get; set; } = new();
}
