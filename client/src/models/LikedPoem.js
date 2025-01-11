import { Poem } from "./Poem.js";

export class LikedPoem extends Poem {
  constructor(data) {
    super(data)
    this.likedPoemId = data.likedPoemId
    this.poemId = data.poemId
    this.accountId = data.accountId
  }
}
