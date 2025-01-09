import { DBItem } from "./DBItem.js"
import { Profile } from "./Profile.js"

export class Poem extends DBItem {
  constructor(data) {
    super(data)
    this.id = data.id
    this.title = data.title
    this.body = data.body
    this.views = data.views
    this.tags = data.tags
    this.likes = data.likes
    this.saves = data.saves
    this.isArchived = data.isArchived
    this.isLiked = data.isLiked
    this.authorId = data.authorId
    this.imgUrl = data.imgUrl
    this.creator = data.creator ? new Profile(data.creator) : null

  }
}


