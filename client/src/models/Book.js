import { DBItem } from "./DBItem.js";
import { Profile } from "./Profile.js";

export class Book extends DBItem {
  constructor(data) {
    super(data)
    this.id = data.id;
    this.title = data.title
    this.creatorId = data.creatorId
    this.isPrivate = data.isPrivate
    this.createdAt = new Date(data.createdAt)
    this.updatedAt = new Date(data.updatedAt)
    this.creatorId = data.creatorId
    this.creator = data.creator
  }
}
