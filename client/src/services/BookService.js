import { AppState } from "@/AppState.js"
import { api } from "./AxiosService.js"
import { Book } from "@/models/Book.js"
import Pop from "@/utils/Pop.js"

class BookService {

  async getMyBooks() {
    const response = await api.get('account/book')
    AppState.myBooks = response.data.map(bookData => new Book(bookData))
  }

  async createBook(bookData) {
    const response = await api.post('api/book', bookData)
    const newBook = new Book(response.data)
    // if (AppState.account?.id == newBook.creatorId) AppState.myBooks.push(newBook)
    AppState.myBooks.push(newBook)
    Pop.success('You created a new book!')
  }
}

export const bookService = new BookService()
