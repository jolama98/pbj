CREATE TABLE IF NOT EXISTS accounts (
    id VARCHAR(255) NOT NULL primary key COMMENT 'primary key',
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
    name varchar(255) COMMENT 'User Name',
    email varchar(255) UNIQUE COMMENT 'User Email',
    picture varchar(255) COMMENT 'User Picture'
) default charset utf8mb4 COMMENT '';

CREATE TABLE IF NOT EXISTS poem(
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    title VARCHAR(255) NOT NULL,
    body VARCHAR(1000) NOT NULL,
    tags VARCHAR(255) NOT NULL,
    isArchived BOOLEAN NOT NULL,
    isLiked BOOLEAN NOT NULL,
    saves INT UNSIGNED NOT NULL DEFAULT 0,
    views INT UNSIGNED NOT NULL DEFAULT 0,
    likes INT UNSIGNED NOT NULL DEFAULT 0,
    authorId VARCHAR(255) NOT NULL,
    FOREIGN KEY (authorId) REFERENCES accounts (id) ON DELETE CASCADE
)



DROP TABLE `likedPoem`;

INSERT INTO
    savedPoem (
        poemId,
        bookId,
        creatorId
    )
VALUES (
        4,
        13,
        '66d109c1258b754bca428053'
    );


INSERT INTO
    poem (
        title,
        body,
        tags,
        isArchived,
        isLiked,
        authorId
    )
VALUES (
        "Soda 2",
        "I sit and wait behind the glass,
A lonely can, time slipping fast.
To be picked is all I crave,
But what if I'm too weak to save?

Will I burst, or will I shine?
Taste the air or cross the line?
I dream of hands to set me free,
But fear what's waiting out for me.,
Th ","#Prince",false,false,'66d109c1258b754bca428053'
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

-- CREATE TABLE IF NOT EXISTS likedPoem (
--     id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
--     createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
--     updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
--     creatorId VARCHAR(255) NOT NULL,
--     poemId INT NOT NULL,
--     FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE,
--     FOREIGN KEY (poemId) REFERENCES poem (id) ON DELETE CASCADE
-- );

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