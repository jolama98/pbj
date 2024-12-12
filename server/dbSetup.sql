CREATE TABLE IF NOT EXISTS accounts(
  id VARCHAR(255) NOT NULL primary key COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name varchar(255) COMMENT 'User Name',
  email varchar(255) UNIQUE COMMENT 'User Email',
  picture varchar(255) COMMENT 'User Picture'
) default charset utf8mb4 COMMENT '';


CREATE TABLE poem(
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

ALTER TABLE poem
ADD image varchar(255)


DROP TABLE poem;
INSERT INTO poem(title, body, tags, isArchived,  authorId) 
VALUES("A Coder’s Quiet Battle", "In the glow of the monitor, late at night,
A coder sits, bathed in flickering light.
The hum of the fan, the clack of the keys,
The only companions in moments like these.

Their mind is a maze, a labyrinth of thought,
Where every solution feels like it’s fought.
Questions loom large, their shadows immense,
“Am I enough? Do I make sense?”

Code sprawls before them, an unbroken thread,
Each error a whisper of lingering dread.
A semicolon missed, a logic gone wrong—
A fragile refrain in an anxious song.

Deadlines approach, their shadows grow tall,
While self-doubt looms like a stormcloud’s call.
The world outside feels distant, removed,
An endless parade of skills to be proved.

Yet, within the chaos, a spark still remains,
A stubborn resolve that outlives the pain.
For every bug fixed, for each challenge met,
They weave a story they won’t soon forget.

And though anxiety whispers, loud and unkind,
There’s courage, too, in the coder’s mind.
To push through the night, to learn, to create,
To build something lasting, to challenge their fate.

The lines of their code may falter, may bend,
But their passion endures—unbroken, their friend.
For in every struggle, a victory hides,
And the anxious coder still quietly thrives.
", "Reflective", false,'66d109c1258b754bca428053'
 )