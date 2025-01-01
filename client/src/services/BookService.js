import { AppState } from "@/AppState.js"
import { api } from "./AxiosService.js"
import { Book } from "@/models/Book.js"

class BookService {

  async getMyBooks() {
    const response = await api.get('account/books')
    AppState.myBooks = response.data.map(bookData => new Book(bookData))
  }
}

export const bookService = new BookService()
