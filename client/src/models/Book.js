import { DBItem } from "./DBItem.js";
import { Profile } from "./Profile.js";

export class Book extends DBItem {
  constructor(data) {
    super(data)
    this.title = data.title
    this.creatorId = data.creatorId
    this.creator = data.creator ? new Profile(data.creator) : null

  }
}
