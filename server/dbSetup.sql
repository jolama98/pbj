/* ============================================================
   PBJ — Poems Social Schema (with Relationship Notes)
   ------------------------------------------------------------

  1→* = one-to-many
  *→1 = many-to-one
  *→* = many-to-many

   High-level ERD (cardinality):

   accounts (id)
      1 ────*  poem.authorId                     (User writes many Poems)
      1 ────*  books.creatorId                   (User owns many Books)
      1 ────*  comments.creatorId                (User writes many Comments)
      1 ────*  likedPoem.creatorId               (User likes many Poems)
      1 ────*  savedPoem.creatorId               (User saves many Poems)

   poem (id)
      * ────1  poem.authorId → accounts(id)
      * ────*  poemGenre(poemId) → genre(…)
      * ────*  likedPoem(poemId) → accounts(…)
      * ────*  savedPoem(poemId) → accounts(…), books(…)
      1 ────*  comments.poemId

   genre (id)
      * ────*  poemGenre.genreId → poem(…)

   books (id)
      * ────*  savedPoem.bookId → poem(…) [acts like a “collection”/playlist]

   Join tables:
      poemGenre:   Poem ↔ Genre (M:N)
      likedPoem:   User  ↔ Poem (M:N)
      savedPoem:   User  ↔ Poem (optionally inside a Book) (M:N)

   Notes on timestamps:
     - Content tables (poem, books, comments) have createdAt + updatedAt.
     - Join tables typically only need createdAt; you kept updatedAt on saved/liked
       which is fine if you want to audit changes.

   ============================================================ */

-- ==========================
-- ACCOUNTS: user profiles
-- REL: 1→* poem (authorId), 1→* books (creatorId),
--      1→* comments (creatorId), 1→* likedPoem (creatorId), 1→* savedPoem (creatorId)
-- ==========================
CREATE TABLE IF NOT EXISTS accounts (
    id VARCHAR(255) NOT NULL PRIMARY KEY COMMENT 'PK: user id (often from auth provider)',
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'When account was created',
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last account update',
    name VARCHAR(255) COMMENT 'User name',
    email VARCHAR(255) UNIQUE COMMENT 'Unique user email',
    picture VARCHAR(255) COMMENT 'Profile picture URL'
) DEFAULT CHARSET utf8mb4 COMMENT 'Accounts / Users table';


-- ==========================
-- POEMS: main content users post
-- REL: *→1 accounts via authorId
--      *→* genre via poemGenre
--      *→* accounts via likedPoem (users who liked this poem)
--      *→* accounts/books via savedPoem (users/books that saved this poem)
--      1→* comments (comments left on this poem)
-- ==========================
CREATE TABLE IF NOT EXISTS poem (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT 'PK for poem',
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'When poem was created',
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last poem update',
    title VARCHAR(255) NOT NULL COMMENT 'Poem title',
    body TEXT NOT NULL COMMENT 'Poem body text',
    tags VARCHAR(255) NOT NULL COMMENT 'Comma-separated tags (can normalize later)',
    isArchived TINYINT(1) NOT NULL DEFAULT 0 COMMENT '0=visible, 1=archived',
    saves INT UNSIGNED NOT NULL DEFAULT 0 COMMENT 'Cached save count (optional)',
    views INT UNSIGNED NOT NULL DEFAULT 0 COMMENT 'View counter (optional)',
    likes INT UNSIGNED NOT NULL DEFAULT 0 COMMENT 'Cached like count (optional)',
    authorId VARCHAR(255) NOT NULL COMMENT 'REL: FK → accounts(id), the poem author',
    FULLTEXT KEY ft_poem_title_body (title, body) COMMENT 'Full-text search on title/body',
    FOREIGN KEY (authorId) REFERENCES accounts (id) ON DELETE CASCADE
) ENGINE=InnoDB COMMENT 'Poems table, core user content';


-- ==========================
-- BOOKS: user-made collections of poems (like playlists/folders)
-- REL: *→1 accounts via creatorId
--      *→* poems via savedPoem(bookId)  [this design uses savedPoem to place poems into books]
-- ==========================
CREATE TABLE IF NOT EXISTS books (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT 'PK for book',
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'When book was created',
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last update to this book',
    title VARCHAR(255) NOT NULL COMMENT 'Book title',
    isPrivate BOOLEAN NOT NULL COMMENT 'If true, only the creator can see it',
    creatorId VARCHAR(255) NOT NULL COMMENT 'REL: FK → accounts(id), owner of the book',
    FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE
) COMMENT 'Books (named collections of poems)';


-- ==========================
-- SAVED POEMS: user bookmarks a poem, optionally inside a specific book
-- REL: *→1 accounts via creatorId (who saved)
--      *→1 poem    via poemId    (what was saved)
--      *→1 books   via bookId    (where it’s saved/organized)
--
-- NOTE: You may want UNIQUE(creatorId, poemId, bookId) if a poem can only appear
--       once per book for a user; or UNIQUE(creatorId, poemId) if “save” is global.
-- ==========================
CREATE TABLE IF NOT EXISTS savedPoem (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT 'PK for savedPoem',
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'When the save happened',
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last update to this save row',
    creatorId VARCHAR(255) NOT NULL COMMENT 'REL: FK → accounts(id), who saved',
    poemId INT NOT NULL COMMENT 'REL: FK → poem(id), what poem was saved',
    bookId INT NOT NULL COMMENT 'REL: FK → books(id), which book the poem was saved into',
    FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE,
    FOREIGN KEY (poemId) REFERENCES poem (id) ON DELETE CASCADE,
    FOREIGN KEY (bookId) REFERENCES books (id) ON DELETE CASCADE
) COMMENT 'Saved poems (bookmarking) — acts as the Poems↔Books linker in this design';


-- ==========================
-- GENRES: master list (Romance, Nature, Sci-Fi, etc.)
-- REL: *→* poems via poemGenre
-- ==========================
CREATE TABLE IF NOT EXISTS genre (
  id INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT 'PK for genre',
  name VARCHAR(100) NOT NULL UNIQUE COMMENT 'Unique genre name'
) COMMENT 'List of available genres';


-- ==========================
-- POEMGENRE: join table for Poem ↔ Genre (many-to-many)
-- REL: *→1 poem via poemId
--      *→1 genre via genreId
-- ==========================
CREATE TABLE IF NOT EXISTS poemGenre (
  id INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT 'PK for poemGenre',
  poemId INT NOT NULL COMMENT 'REL: FK → poem(id)',
  genreId INT NOT NULL COMMENT 'REL: FK → genre(id)',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'When the genre was attached to the poem',
  UNIQUE KEY uq_poem_genre (poemId, genreId) COMMENT 'Prevents duplicate tagging',
  INDEX ix_poem_genre_poem (poemId),
  INDEX ix_poem_genre_genre (genreId),
  FOREIGN KEY (poemId) REFERENCES poem (id) ON DELETE CASCADE,
  FOREIGN KEY (genreId) REFERENCES genre (id) ON DELETE CASCADE
) COMMENT 'Join: Poems ↔ Genres (M:N)';


-- ==========================
-- LIKEDPOEM: users liking poems (many-to-many)
-- REL: *→1 accounts via creatorId (who liked)
--      *→1 poem    via poemId    (what was liked)
-- ==========================
CREATE TABLE IF NOT EXISTS likedPoem (
  id INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT 'PK for likedPoem',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'When the like happened',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Updated if you ever edit this row',
  creatorId VARCHAR(255) NOT NULL COMMENT 'REL: FK → accounts(id), the liker',
  poemId INT NOT NULL COMMENT 'REL: FK → poem(id), the liked poem',
  UNIQUE KEY uq_like (creatorId, poemId) COMMENT 'Prevents double-like by same user on same poem',
  INDEX ix_like_user (creatorId),
  INDEX ix_like_poem (poemId),
  FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE,
  FOREIGN KEY (poemId) REFERENCES poem (id) ON DELETE CASCADE
) COMMENT 'Join: Users ↔ Poems (likes)';


-- ==========================
-- COMMENTS: users commenting on poems
-- REL: *→1 accounts via creatorId
--      *→1 poem    via poemId
-- ==========================
CREATE TABLE IF NOT EXISTS comments (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT 'PK for comment',
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'When the comment was created',
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last edit time for this comment',
    body VARCHAR(255) NOT NULL COMMENT 'Comment text',
    creatorId VARCHAR(255) NOT NULL COMMENT 'REL: FK → accounts(id), commenter',
    poemId INT NOT NULL COMMENT 'REL: FK → poem(id), which poem is being commented on',
    FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE,
    FOREIGN KEY (poemId) REFERENCES poem (id) ON DELETE CASCADE
) COMMENT 'Comments left by users on poems';




DROP TABLE IF EXISTS `cards`;

-- 1) Sunrise Promise
INSERT INTO poem (
  title,
  body,
  tags,
  authorId
) VALUES (
  'Whispers in the Rain',
  'The rain taps soft against the glass,
   A thousand voices in the night.
   Each drop a memory drifting past,
   Each echo carrying fragile light.',
  'nature,melancholy,reflection',
  '6691cd264de80d398f94368a'   -- must match an existing accounts.id
);
