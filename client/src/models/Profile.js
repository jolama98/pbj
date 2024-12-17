import { DBItem } from "./DBItem.js";

export class Profile extends DBItem {
  constructor(data) {
    super(data)
    this.name = data.name
    this.picture = data.picture
    this.coverImg = data.coverImg
  }
}
