CREATE TABLE IF NOT EXISTS accounts (
    id VARCHAR(255) NOT NULL primary key COMMENT 'primary key',
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
    name varchar(255) COMMENT 'User Name',
    email varchar(255) UNIQUE COMMENT 'User Email',
    picture varchar(255) COMMENT 'User Picture'
) default charset utf8mb4 COMMENT '';

CREATE TABLE poem (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    title VARCHAR(255) NOT NULL,
    body VARCHAR(1000) NOT NULL,
    tags VARCHAR(255) NOT NULL,
    isArchived BOOLEAN NOT NULL,
    saves INT UNSIGNED NOT NULL DEFAULT 0,
    views INT UNSIGNED NOT NULL DEFAULT 0,
    likes INT UNSIGNED NOT NULL DEFAULT 0,
    authorId VARCHAR(255) NOT NULL,
    FOREIGN KEY (authorId) REFERENCES accounts (id) ON DELETE CASCADE
    -- FOREIGN KEY (saved) REFERENCES 
)

ALTER TABLE poem ADD image varchar(255)

DROP TABLE poem;

INSERT INTO
    poem (
        title,
        body,
        tags,
        isArchived,
        authorId
    )
VALUES (
        "knnni",
        "In a kingdom where the stars aligned,
A prince with words both sweet and kind,
Met a princess, her heart so bright,
Their bond grew strong beneath the night.
The prince, with every gentle phrase,
Set her heart and soul ablaze.
She loved the way he spoke so true,
And all the things he dared to do.
But there was one, a knight so bold,
With eyes that watched, a heart of gold.
He guarded her with fierce intent,
A bond that neither could repent.
Each night he waited, filled with dread,
While thoughts of her and the prince were spread.
For though he loved her as a friend,
He feared the prince would be her end.
",
        "#Prince
#Knight
#LoveTriangle
#Kingdom
#Romance
#Conflict
#Loyalty
#Heartfelt
#EmotionalTug
",
        false,
        '66d109c1258b754bca428053'
    )

CREATE TABLE savedPoem (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    creatorId VARCHAR(255) NOT NULL,
    poemId INT NOT NULL,
    bookId INT NOT NULL,
    FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE,
    FOREIGN KEY (poemId) REFERENCES poem (id) ON DELETE CASCADE,
    FOREIGN KEY (bookId) REFERENCES books (id) ON DELETE CASCADE
)

DROP TABLE savedPoem

CREATE TABLE likedPoem (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    creatorId VARCHAR(255) NOT NULL,
    poemId INT NOT NULL,
    FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE,
    FOREIGN KEY (poemId) REFERENCES poem (id) ON DELETE CASCADE
)

CREATE TABLE books (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    title VARCHAR(255) NOT NULL,
    creatorId VARCHAR(255) NOT NULL,
    FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE
)

ALTER TABLE books ADD isPrivate BOOLEAN NOT NULL

CREATE TABLE comments (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    body VARCHAR(255) NOT NULL,
    title VARCHAR(255) NOT NULL,
    creatorId VARCHAR(255) NOT NULL,
    poemId INT NOT NULL,
    FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE,
    FOREIGN KEY (poemId) REFERENCES poem (id) ON DELETE CASCADE
)

SELECT * FROM poem WHERE Title LIKE OR Body LIKE @SearchTerm;