import { Account } from "./Account.js"
import { Poem } from "./Poem.js"


export class LikedPoem {
  constructor(data) {
    this.id = data.id || data._id
    this.accountId = data.accountId
    this.poemId = data.poemId
  }
}

export class LikedPoemProfile extends LikedPoem {
  constructor(data) {
    super(data)
    this.account = new Account(data.account)
  }
}

export class Liked extends LikedPoem {
  constructor(data) {
    super(data)
    this.poem = new Poem(data.poem)
  }
}
