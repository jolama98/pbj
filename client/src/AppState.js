import { reactive } from 'vue'
import { Poem } from './models/Poem.js'
import { Book } from './models/Book.js'
import { LikedPoem } from './models/LikedPoem.js'
import { SavedPoem } from './models/SavedPoem.js'
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

  /**@type {import('./models/LikedPoem.js').LikedPoem[]} */
  likedPoem: [],

  /**@type {SavedPoem[]} */
  savedPoem: [],
  /**@type {import('./models/Comments.js') .Comment[]} */
  comments: [],
  /**@type {Comment[]} */
  activePoemComments: []

})

