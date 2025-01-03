import { Account } from "./Account.js";
import { Book } from "./Book.js";
import { Poem } from "./Poem.js";

export class SavedPoem {
  constructor(data) {
    this.id = data.id;
    this.poemId = data.poemId;
    this.bookId = data.bookId;
  }
}

export class SavedPoemPoem extends Poem {
  constructor(data) {
    super(data);
    this.savedPoemId = data.savedPoemId;
    this.bookId = data.bookId;
  }
}

export class BookProfile extends SavedPoem {
  constructor(data) {
    super(data)
    this.profile = new Account(data.profile)
  }
}

export class SavedPoemBook extends SavedPoem {
  constructor(data) {
    super(data)
    this.book = new Book(data.book)
  }
}
