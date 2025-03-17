import { DBItem } from "./DBItem.js";

export class Comment extends DBItem {
  constructor(data) {
    super(data)
    this.id = data.id
    this.poemId = data.poemId
    this.body = data.body
    this.creatorId = data.creatorId
    this.creator = data.creator
  }
}
