import { Account } from "./Account.js"
import { Poem } from "./Poem.js"


export class Like {
  constructor(data) {
    this.id = data.id || data._id
    this.accountId = data.accountId
    this.poemId = data.poemId
  }
}

export class LikedPoemProfile extends Like {
  constructor(data) {
    super(data)
    this.account = new Account(data.account)
  }
}

export class LikedPoem extends Like {
  constructor(data) {
    super(data)
    this.poem = new Poem(data.poem)
  }
}
