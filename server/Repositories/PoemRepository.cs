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
internal IEnumerable<Poem> SearchPoems(string booleanQuery, string plain, string tagExact, int skip, int take)
{
    // BOOLEAN MODE (ranked) + LIKE fallbacks + pagination
    const string fullTextSql = @"
SELECT
  p.*,
  a.*,
  (
      (MATCH(p.title) AGAINST (@q IN BOOLEAN MODE)) * 3.0
    + (MATCH(p.tags)  AGAINST (@q IN BOOLEAN MODE)) * 2.5
    + (MATCH(p.body)  AGAINST (@q IN BOOLEAN MODE)) * 1.0
    + COALESCE((
        SELECT MAX(MATCH(g.name) AGAINST (@q IN BOOLEAN MODE))
        FROM poemGenre pg
        JOIN genre g ON g.id = pg.genreId
        WHERE pg.poemId = p.id
      ), 0) * 2.0
    + CASE
        WHEN @tagExact IS NOT NULL AND @tagExact <> ''
             AND FIND_IN_SET(@tagExact, REPLACE(LOWER(p.tags), ' ', '')) > 0
        THEN 1.0 ELSE 0
      END * 1.5
  ) AS rank_score
FROM poem p
JOIN accounts a ON a.id = p.authorId
WHERE
      MATCH(p.title, p.body, p.tags) AGAINST (@q IN BOOLEAN MODE)
   OR EXISTS (
        SELECT 1
        FROM poemGenre pg
        JOIN genre g ON g.id = pg.genreId
        WHERE pg.poemId = p.id
          AND MATCH(g.name) AGAINST (@q IN BOOLEAN MODE)
      )
   OR p.title LIKE CONCAT('%', @plain, '%')
   OR p.body  LIKE CONCAT('%', @plain, '%')
   OR p.tags  LIKE CONCAT('%', @plain, '%')
   OR EXISTS (
        SELECT 1
        FROM poemGenre pg
        JOIN genre g ON g.id = pg.genreId
        WHERE pg.poemId = p.id
          AND g.name LIKE CONCAT('%', @plain, '%')
      )
ORDER BY rank_score DESC, p.createdAt DESC
LIMIT @take OFFSET @skip;";

    try
    {
        return _db.Query<Poem, Profile, Poem>(
            fullTextSql,
            JoinCreator,
            new { q = booleanQuery, plain, tagExact, skip, take },
            splitOn: "id" // first id is poem.id, second id is accounts.id
        );
    }
    catch
    {
        // LIKE-only fallback if FULLTEXT is unavailable or misindexed in dev
        const string likeSql = @"
SELECT
  p.*,
  a.*
FROM poem p
JOIN accounts a ON a.id = p.authorId
WHERE
      p.title LIKE CONCAT('%', @plain, '%')
   OR p.body  LIKE CONCAT('%', @plain, '%')
   OR p.tags  LIKE CONCAT('%', @plain, '%')
   OR EXISTS (
        SELECT 1
        FROM poemGenre pg
        JOIN genre g ON g.id = pg.genreId
        WHERE pg.poemId = p.id
          AND g.name LIKE CONCAT('%', @plain, '%')
      )
ORDER BY p.createdAt DESC
LIMIT @take OFFSET @skip;";

        return _db.Query<Poem, Profile, Poem>(
            likeSql,
            JoinCreator,
            new { plain, skip, take },
            splitOn: "id"
        );
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
