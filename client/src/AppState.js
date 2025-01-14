import { reactive } from 'vue'
import { Poem } from './models/Poem.js'
import { Book } from './models/Book.js'
// import { SavedPoemBook } from './models/SavedPoem.js'

// NOTE AppState is a reactive object to contain app level data
export const AppState = reactive({
  /**@type {import('@bcwdev/auth0provider-client').Identity} */
  identity: null,
  /** @type {import('./models/Account.js').Account} user info from the database*/
  account: null,
  /**@type {import('./models/Poem.js').Poem[]} */
  poems: [],
  /**@type {Poem} */
  poemById: null,
  /**@type {Poem} */
  setActivePoem: null,
  poemQuery: '',
  /**@type {import('./models/Profile.js').Profile} */
  activeProfile: null,
  /**@type {import('./models/Book.js').Book[]} */
  myBooks: [],
  /** @type {Book[]} */
  profileBooks: [],
  /**@type {import('./models/Like.js').LikedPoem[]} */
  likedPoem: [],

})

