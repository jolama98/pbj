CREATE TABLE IF NOT EXISTS accounts (
    id VARCHAR(255) NOT NULL primary key COMMENT 'primary key',
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
    name varchar(255) COMMENT 'User Name',
    email varchar(255) UNIQUE COMMENT 'User Email',
    picture varchar(255) COMMENT 'User Picture'
) default charset utf8mb4 COMMENT '';


-- Example: extend poem table
ALTER TABLE poem 
ADD COLUMN genre VARCHAR(100) NOT NULL DEFAULT 'General';


CREATE TABLE IF NOT EXISTS poem(
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    title VARCHAR(255) NOT NULL,
    body TEXT NOT NULL, 
    tags VARCHAR(255) NOT NULL,
  isArchived TINYINT(1) NOT NULL DEFAULT 0,   -- BOOLEAN alias; give a default
    saves INT UNSIGNED NOT NULL DEFAULT 0,
    views INT UNSIGNED NOT NULL DEFAULT 0,
    likes INT UNSIGNED NOT NULL DEFAULT 0,
    authorId VARCHAR(255) NOT NULL,
  FULLTEXT KEY ft_poem_title_body (title, body),  
    FOREIGN KEY (authorId) REFERENCES accounts (id) ON DELETE CASCADE
);



DROP TABLE `poem`;
DROP TABLE `comments`; 



INSERT INTO poem (
  title,
  body,
  tags,
  isArchived,
  authorId
) VALUES (
  'Echoes of the Sea',
  'Waves unravel secrets on the shore, 
   shells hold the memory of distant storms. 
   Salt and wind weave through the air, 
   carrying songs older than the tide.',
  'ocean,travel,memory',
  0,                       -- not archived
  '66d109c1258b754bca428053'               -- must match an id in accounts
);




CREATE TABLE IF NOT EXISTS savedPoem (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    creatorId VARCHAR(255) NOT NULL,
    poemId INT NOT NULL,
    bookId INT NOT NULL,
    FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE,
    FOREIGN KEY (poemId) REFERENCES poem (id) ON DELETE CASCADE,
    FOREIGN KEY (bookId) REFERENCES books (id) ON DELETE CASCADE
);
DROP TABLE `likedPoem`;


-- 1) Genres master table
CREATE TABLE IF NOT EXISTS genre (
  id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(100) NOT NULL UNIQUE
);

-- 2) Poem ↔ Genre (many-to-many)
CREATE TABLE IF NOT EXISTS poemGenre (
  id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  poemId INT NOT NULL,
  genreId INT NOT NULL,
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
  UNIQUE KEY uq_poem_genre (poemId, genreId),
  INDEX ix_poem_genre_poem (poemId),
  INDEX ix_poem_genre_genre (genreId),
  FOREIGN KEY (poemId) REFERENCES poem (id) ON DELETE CASCADE,
  FOREIGN KEY (genreId) REFERENCES genre (id) ON DELETE CASCADE
);

-- 3) Likes (user ↔ poem)
CREATE TABLE IF NOT EXISTS likedPoem (
  id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  creatorId VARCHAR(255) NOT NULL,
  poemId INT NOT NULL,
  UNIQUE KEY uq_like (creatorId, poemId),        -- prevents double-like
  INDEX ix_like_user (creatorId),
  INDEX ix_like_poem (poemId),
  FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE,
  FOREIGN KEY (poemId) REFERENCES poem (id) ON DELETE CASCADE
);


INSERT INTO
    likedPoem (poemId, creatorId)
VALUES (
        3,
        '66d109c1258b754bca428053'
    );


CREATE TABLE IF NOT EXISTS books (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    title VARCHAR(255) NOT NULL,
    isPrivate BOOLEAN NOT NULL,
    creatorId VARCHAR(255) NOT NULL,
    FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE
);

DROP TABLE poem

ALTER TABLE books ADD isPrivate BOOLEAN NOT NULL

CREATE TABLE IF NOT EXISTS comments (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    body VARCHAR(255) NOT NULL,
    creatorId VARCHAR(255) NOT NULL,
    poemId INT NOT NULL,
    FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE,
    FOREIGN KEY (poemId) REFERENCES poem (id) ON DELETE CASCADE
);

SELECT * FROM poem WHERE Title LIKE OR Body LIKE @SearchTerm;