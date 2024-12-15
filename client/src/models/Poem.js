import { Account } from "./Account.js"

export class Poem {
  constructor(data){
    this.id = data.id
    this.title = data.title
    this.body = data.body
    this.views = data.views
    this.tags = data.tags
    this.likes = data.likes
    this.saves = data.saves
    this.isArchived = data.isArchived
    this.authorId = data.authorId
    this.imgUrl = data.imgUrl
    this.creator = data.creator ? new Account(data.creator) : null
  }
}