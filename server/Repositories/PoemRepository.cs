namespace pbj.Repositories;

/*
===============================================================================
PoemRepository — Data Access (Dapper + MySQL)
-------------------------------------------------------------------------------
READ THIS FIRST (future-you):

- This layer ONLY talks to the DB. No business rules here (ownership, view
  increments, etc.) — keep that in the Service.
- All SELECTs that join accounts return Poem + its Creator (Profile).
- Dapper multi-mapping is used to hydrate Poem.Creator. We rely on splitOn: "id"
  because both result sets have an "id" column (poem.id then accounts.id).

FIXED ISSUES FROM OLD VERSION:
- Removed non-existent column "isLiked" from INSERT (isLiked is computed per-user).
- Removed GROUP BY misuses (no aggregates → use ORDER BY).
- Corrected GetPoemByProfileId join (poem.authorId, not poem.creatorId).
- Added ORDER BYs and LIMITs where sensible.
- SearchPoems now has a FULLTEXT path (fast) and falls back to LIKE if needed.
- UpdatePoem updates only fields we expect to change (views/likes/saves/title/body/isArchived).

HANDY QUERIES:
- After INSERT, use LAST_INSERT_ID() to re-select the created row (with join).
- When multi-mapping, always pass splitOn: "id" (or alias the second id if you prefer).

SCHEMA EXPECTATIONS (poem):
  id (PK, int), createdAt, updatedAt, title (req), body (req), tags,
  isArchived (tinyint(1)), saves, views, likes, authorId (FK)
===============================================================================
*/
public class PoemRepository
{
    private readonly IDbConnection _db;

    public PoemRepository(IDbConnection db)
    {
        _db = db;
    }

    // ------------------------------------------------------------------------
    // CREATE
    // Note: isLiked is NOT a column on poem (it's per-user), so do NOT insert it.
    // ------------------------------------------------------------------------
    internal Poem CreatePoem(Poem poemData)
    {
        string sql = @"
INSERT INTO poem
  (title, body, tags, isArchived, saves, views, likes, authorId)
VALUES
  (@Title, @Body, @Tags, @IsArchived, @Saves, @Views, @Likes, @AuthorId);

SELECT
  poem.*,
  accounts.*
FROM poem
JOIN accounts ON accounts.id = poem.authorId
WHERE poem.id = LAST_INSERT_ID();
";
        Poem poem = _db.Query<Poem, Profile, Poem>(
            sql,
            JoinCreator,
            poemData,
            splitOn: "id"
        ).FirstOrDefault();

        return poem;
    }

    // ------------------------------------------------------------------------
    // READ: All poems (simple list). Order by newest id by default.
    // Add paging in service/controller if needed.
    // ------------------------------------------------------------------------
    internal List<Poem> GetAllPoems()
    {
        string sql = @"
SELECT
  poem.*,
  accounts.*
FROM poem
JOIN accounts ON accounts.id = poem.authorId
ORDER BY poem.id DESC;
";
        List<Poem> poems = _db.Query<Poem, Profile, Poem>(
            sql,
            JoinCreator,
            splitOn: "id"
        ).ToList();

        return poems;
    }

    // ------------------------------------------------------------------------
    // READ: One poem by id
    // IMPORTANT: No GROUP BY (we aren't aggregating). Limit 1 for safety.
    // ------------------------------------------------------------------------
    internal Poem GetPoemById(int poemId)
    {
        string sql = @"
SELECT
  poem.*,
  accounts.*
FROM poem
JOIN accounts ON accounts.id = poem.authorId
WHERE poem.id = @poemId
LIMIT 1;
";
        Poem poem = _db.Query<Poem, Profile, Poem>(
            sql,
            JoinCreator,
            new { poemId },
            splitOn: "id"
        ).FirstOrDefault();

        return poem;
    }

    // ------------------------------------------------------------------------
    // READ: All poems by a specific author (profileId == accounts.id)
    // FIX: join uses poem.authorId (not poem.creatorId).
    // ------------------------------------------------------------------------
    internal List<Poem> GetPoemByProfileId(string profileId)
    {
        string sql = @"
SELECT
  poem.*,
  accounts.*
FROM poem
JOIN accounts ON accounts.id = poem.authorId
WHERE accounts.id = @profileId
ORDER BY poem.id DESC;
";
        List<Poem> poems = _db.Query<Poem, Profile, Poem>(
            sql,
            JoinCreator,
            new { profileId },
            splitOn: "id"
        ).ToList();

        return poems;
    }

    // ------------------------------------------------------------------------
    // SEARCH: FULLTEXT if available; fallback to LIKE otherwise
    // NOTE: Requires FULLTEXT KEY on (title, body).
    // ------------------------------------------------------------------------
    internal List<Poem> SearchPoems(string searchTerm)
    {
        // Preferred (fast) FULLTEXT path:
        string fullTextSql = @"
SELECT
  poem.*,
  accounts.*
FROM poem
JOIN accounts ON accounts.id = poem.authorId
WHERE MATCH(poem.title, poem.body) AGAINST (@q IN NATURAL LANGUAGE MODE)
ORDER BY poem.id DESC;
";
        try
        {
            return _db.Query<Poem, Profile, Poem>(
                fullTextSql,
                JoinCreator,
                new { q = searchTerm },
                splitOn: "id"
            ).ToList();
        }
        catch
        {
            // Fallback to LIKE if FULLTEXT isn't available (dev envs, etc.)
            string likeSql = @"
SELECT
  poem.*,
  accounts.*
FROM poem
JOIN accounts ON accounts.id = poem.authorId
WHERE
  poem.title LIKE CONCAT('%', @q, '%')
  OR poem.body LIKE CONCAT('%', @q, '%')
  OR poem.tags LIKE CONCAT('%', @q, '%')
ORDER BY poem.id DESC;
";
            return _db.Query<Poem, Profile, Poem>(
                likeSql,
                JoinCreator,
                new { q = searchTerm },
                splitOn: "id"
            ).ToList();
        }
    }

    // ------------------------------------------------------------------------
    // UPDATE: Update mutable fields. updatedAt is handled by DB trigger (ON UPDATE)
    // Only update what you expect to change. Add Saves/IsArchived here as needed.
    // ------------------------------------------------------------------------
    internal void UpdatePoem(Poem poemToUpdate)
    {
        string sql = @"
UPDATE poem
SET
  title      = @Title,
  body       = @Body,
  tags       = @Tags,
  isArchived = @IsArchived,
  saves      = @Saves,
  views      = @Views,
  likes      = @Likes
WHERE id = @Id
LIMIT 1;
";
        int rows = _db.Execute(sql, poemToUpdate);
        if (rows == 0) throw new Exception("UPDATE FAILED (no rows)");
        if (rows > 1) throw new Exception("UPDATE AFFECTED MULTIPLE ROWS (unexpected)");
    }

    // ------------------------------------------------------------------------
    // DELETE
    // ------------------------------------------------------------------------
    internal void DestroyPoem(int poemId)
    {
        string sql = "DELETE FROM poem WHERE id = @poemId LIMIT 1;";
        int rows = _db.Execute(sql, new { poemId });

        if (rows == 0) throw new Exception("DELETE FAILED (no rows)");
        if (rows > 1) throw new Exception("DELETE AFFECTED MULTIPLE ROWS (unexpected)");
    }

    // ------------------------------------------------------------------------
    // Helper: map Poem + Profile (Creator)
    // ------------------------------------------------------------------------
    private Poem JoinCreator(Poem poem, Profile profile)
    {
        poem.Creator = profile;
        return poem;
    }
}
