import { Book } from '@/models/Book.js'
import { AppState } from '../AppState.js'
import { Account } from '../models/Account.js'
import { logger } from '../utils/Logger.js'
import { api } from './AxiosService.js'

class AccountService {



  async updateAccount(accountData) {
    const response = await api.put('/account', accountData)
    logger.log('UPDATED ACCOUNT 🤵', response.data)
    AppState.account = new Account(response.data)
  }


  async getAccount() {
    try {
      const res = await api.get('/account')
      AppState.account = new Account(res.data)
    } catch (err) {
      logger.error('HAVE YOU STARTED YOUR SERVER YET???', err)
    }
  }

  async getAccountBook() {
    const response = await api.get('account/book')
    // logger.log("😂", response.data)
    const myBooks = response.data.map(myBooks => new Book(myBooks))
    AppState.profileBooks = myBooks
  }

  async getLikedPoemsByProfileId() {
    const response = await api.get('account/likedPoem')
    // logger.log('Got poems for profile', response.data)
    const likedPoems = response.data.map(likedPoems => new Book(likedPoems))
    AppState.likedPoem = likedPoems
  }

}

export const accountService = new AccountService()
