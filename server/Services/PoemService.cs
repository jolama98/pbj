namespace pbj.Services;

/*
===============================================================================
PoemService — Business Logic Layer for Poems
-------------------------------------------------------------------------------
CHEAT SHEET:

- This sits between the Controller and Repository.
- It enforces business rules:
  * Only the poem’s author can update/delete it.
  * Increments views when a poem is fetched.
  * Handles validation (e.g., no empty search term).
- The repository (_poemRepository) actually runs SQL.
- The commentService (_commentService) can be used to fetch comments related
  to poems (not fully wired in this snippet).

SERVICE CONTRACT (what controller expects here):
  - List:    GetAllPoems()
  - Create:  CreatePoem(Poem poemData)
  - Delete:  DestroyPoem(int poemId, string userId) — ownership check
  - Read:    GetPoemById(int poemId) → increments view count
  - Read:    GetPoemById(int poemId, string userId) → overloaded, lets you pass
              the current user so you can compute IsLiked (not yet implemented)
  - Update:  UpdatePoem(int poemId, string userId, Poem poemData) — ownership check
  - ByUser:  GetPoemByProfileId(string profileId) → poems for a given account
  - Search:  SearchForPoems(string keyword) → uses FULLTEXT in repo

TO DO (future features):
  - Fill IsLiked on GetPoemById if userId provided (join likedPoem table).
  - Fill Genres list (join poemGenre + genre).
  - Include comment counts or actual comments if needed.
===============================================================================
*/
public class PoemService
{
    private readonly PoemRepository _poemRepository;   // Data access for poems
    private readonly CommentService _commentService;   // (Future) for fetching comments

    public PoemService(PoemRepository poemRepository, CommentService commentService)
    {
        _poemRepository = poemRepository;
        _commentService = commentService;
    }

    // =========================================================================
    // Get all poems (no filters here; controller-level method may add paging/filters)
    // =========================================================================
    internal List<Poem> GetAllPoems(int skip, int take, string authorId, string tag, string genre)
    {
        List<Poem> poems = _poemRepository.GetAllPoems();
        return poems;
    }



    // =========================================================================
    // Create a new poem
    // - Controller sets AuthorId from current user before calling.
    // =========================================================================
    internal Poem CreatePoem(Poem poemData)
    {
        Poem poem = _poemRepository.CreatePoem(poemData);
        return poem;
    }

    // =========================================================================
    // Delete poem
    // - Only the author can delete their poem.
    // - Throws if userId != AuthorId.
    // =========================================================================
    internal string DestroyPoem(int poemId, string userId)
    {
        Poem poem = GetPoemById(poemId);
        if (poem.AuthorId != userId)
        {
            throw new Exception("You did not create this poem; cannot delete.");
        }

        _poemRepository.DestroyPoem(poemId);
        return "Poem was deleted!";
    }

    // =========================================================================
    // Get poem by id (anonymous/any user)
    // - Increments view count each time.
    // - Returns Exception if poem not found.
    // =========================================================================
    internal Poem GetPoemById(int poemId)
    {
        Poem poem = _poemRepository.GetPoemById(poemId)
            ?? throw new Exception($"No Poem found with id {poemId}");

        poem.Views += 1;                 // business rule: count views
        _poemRepository.UpdatePoem(poem); // persist increment
        return poem;
    }

    // =========================================================================
    // Get poem by id (with current user context)
    // - Overload lets us compute extra flags (e.g., IsLiked).
    // - Currently just calls GetPoemById; add IsLiked here later.
    // =========================================================================
    internal Poem GetPoemById(int poemId, string userId)
    {
        Poem poem = GetPoemById(poemId);
        // TODO: set poem.IsLiked = _poemRepository.HasUserLiked(poemId, userId);
        return poem;
    }

    // =========================================================================
    // Update poem
    // - Only author can update.
    // - Currently only updates Title and Body (expand if needed).
    // =========================================================================
    internal Poem UpdatePoem(int poemId, string userId, Poem poemData)
    {
        Poem poemToUpdate = GetPoemById(poemId);

        if (poemToUpdate.AuthorId != userId)
        {
            throw new Exception("Forbidden: you cannot update a poem you did not create.");
        }

        // Use null-coalescing to avoid overwriting with nulls
        poemToUpdate.Title = poemData.Title ?? poemToUpdate.Title;
        poemToUpdate.Body = poemData.Body ?? poemToUpdate.Body;

        _poemRepository.UpdatePoem(poemToUpdate);
        return poemToUpdate;
    }

    // =========================================================================
    // Get all poems by a specific profileId (authorId)
    // =========================================================================
    internal List<Poem> GetPoemByProfileId(string profileId)
    {
        List<Poem> poems = _poemRepository.GetPoemByProfileId(profileId);
        return poems;
    }

    // =========================================================================
    // Search poems by keyword
    // - Throws if keyword is empty/whitespace.
    // - Repository uses FULLTEXT (title, body).
    // =========================================================================
    internal List<Poem> SearchForPoems(string query, int skip, int take)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("Search term cannot be empty or whitespace.");
        }

        return _poemRepository.SearchPoems(query);
    }

   
}
