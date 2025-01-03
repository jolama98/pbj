export class Book {
  constructor(data) {
    this.id = data.id;
    this.title = data.title
    this.creatorId = data.creatorId
    this.isPrivate = data.isPrivate
    this.createdAt = new Date(data.createdAt)
    this.updatedAt = new Date(data.updatedAt)
    this.creatorId = data.creatorId
    // this.creator = data.creator ? new Profile(data.creator) : null
  }
}
