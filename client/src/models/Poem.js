import { DBItem } from './DBItem.js'
import { Profile } from './Profile.js'
import { LikedPoem } from './LikedPoem.js'

export class Poem extends DBItem {
  constructor(data) {
    super(data)

    this.id = data.id
    this.title = data.title
    this.body = data.body
    this.views = data.views
    this.tags = data.tags
    this.genre = data.genre

    this.saves = data.saves
    this.isArchived = data.isArchived

    // ✅ keep the DB like-count separate from liked objects
    this.likeCount = data.likes ?? 0  

    this.isLiked = data.isLiked ?? false

    this.authorId = data.authorId

    /** @type {Profile} */
    this.creator = data.creator

    /** @type {LikedPoem[]} */
    this.likedBy = data.likedBy ?? []   // array of users who liked it
  }
}
