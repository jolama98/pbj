// PoemsService.js
// ----------------------------------------------------------------------------
// FUTURE-YOU NOTES (read me in 3 months):
// - This service wraps Axios calls for Poems and updates AppState.
// - Conventions:
//   * Always map API payloads into model classes (Poem) before storing.
//   * Use try/catch and log errors; surface user-facing messages with Pop.
//   * Keep method names camelCase (components should import and call these).
// - Endpoints expected (from PoemController):
//   GET    /api/poem                        → list poems
//   GET    /api/poem/{id}                   → single poem (also increments views)
//   GET    /api/poem/search?query=...       → search (FULLTEXT/LIKE server-side)
//   POST   /api/poem                        → create (auth required)
//   DELETE /api/poem/{id}                   → delete (auth + ownership)
// - AppState fields used:
//   AppState.poems        → list for feeds
//   AppState.poemById     → currently viewed poem
//   AppState.poemQuery    → current search query string (UI can bind to this)
//   AppState.savedPoem    → saved/bookmarked poems list (from /api/savedPoems)
// ----------------------------------------------------------------------------

import { logger } from '@/utils/Logger.js'
import { api } from './AxiosService.js'
import { Poem } from '@/models/Poem.js'
import { AppState } from '@/AppState.js'
import Pop from '@/utils/Pop.js'

class PoemsService {
  // Keep a reference for canceling in-flight searches (user types quickly)
  /** @type {AbortController | null} */
  _searchAbort = null

  // ---------------------------------------------
  // SEARCH
  // - Server requires a non-empty "query"
  // - We guard empty queries to avoid 400s and needless requests
  // - Uses AbortController so rapid re-typing cancels older requests
  // ---------------------------------------------
  async searchPoem(query) {
    try {
      const q = (query ?? '').trim()
      AppState.poemQuery = q

      if (!q) {
        // Empty → treat as "clear" and load default feed
        await this.getAllPoems()
        return
      }

      // Cancel prior search if still running
      if (this._searchAbort) this._searchAbort.abort()
      this._searchAbort = new AbortController()

      const res = await api.get('api/poem/search', {
        params: { query: q },
        signal: this._searchAbort.signal
      })

      logger.log('Searched poems - poems service', res.data)
      AppState.poems = res.data.map(p => new Poem(p))
    } catch (error) {
      // Ignore AbortError (user typed again)
      if (error?.name === 'CanceledError' || error?.name === 'AbortError') return
      logger.error('Error searching poems:', error)
      Pop.error(error?.response?.data || 'Search failed')
      throw error
    } finally {
      this._searchAbort = null
    }
  }

  // ---------------------------------------------
  // Clear search and reload default feed
  // ---------------------------------------------
  async clearSearchQuery() {
    AppState.poemQuery = ''
    await this.getAllPoems()
  }

  // ---------------------------------------------
  // LIST
  // - Could support paging later with params { skip, take, authorId, tag, genre }
  // ---------------------------------------------
  async getAllPoems(params = null) {
    try {
      const res = await api.get('api/poem', { params: params ?? {} })
      AppState.poems = res.data.map(p => new Poem(p))
    } catch (error) {
      logger.error('Error loading poems:', error)
      Pop.error(error?.response?.data || 'Failed to load poems')
      throw error
    }
  }

  // ---------------------------------------------
  // READ ONE
  // - Server increments views on read
  // - Store raw object to AppState.poemById (model if you prefer)
  // ---------------------------------------------
  async getPoemById(poemId) {
    try {
      AppState.poemById = null
      const res = await api.get(`api/poem/${poemId}`)
      AppState.poemById = new Poem(res.data)
    } catch (error) {
      logger.error('Error getting poem by id:', error)
      Pop.error(error?.response?.data || 'Failed to load poem')
      throw error
    }
  }

  // ---------------------------------------------
  // CREATE
  // - Requires auth; server sets AuthorId from token
  // - Prepend new poem to feed for snappy UI
  // ---------------------------------------------
  async createPoem(poemData) {
    try {
      const res = await api.post('api/poem', poemData)
      const created = new Poem(res.data)
      AppState.poems = [created, ...(AppState.poems ?? [])]
      return created
    } catch (error) {
      logger.error('Error creating poem:', error)
      Pop.error(error?.response?.data || 'Failed to create poem')
      throw error
    }
  }

  // ---------------------------------------------
  // DELETE
  // - Requires auth + ownership; server returns 204 No Content
  // - Optimistically remove from AppState list
  // ---------------------------------------------
  async deletePoem(poemId) {
    try {
      await api.delete(`api/poem/${poemId}`)
      // remove from lists if present
      const i = AppState.poems.findIndex(p => p.id == poemId)
      if (i !== -1) AppState.poems.splice(i, 1)

      if (AppState.poemById?.id == poemId) {
        AppState.poemById = null
      }

      Pop.success('Poem deleted')
    } catch (error) {
      logger.error('Error deleting poem:', error)
      Pop.error(error?.response?.data || 'Failed to delete poem')
      throw error
    }
  }

  // ---------------------------------------------
  // SAVED POEMS (bookmark list)
  // - Endpoint expected: GET /api/savedPoems
  // - Maps each item to Poem model for consistent UI
  // ---------------------------------------------
  async getAllSavedPoem() {
    try {
      const res = await api.get('api/savedPoems')
      AppState.savedPoem = res.data.map(p => new Poem(p))
    } catch (error) {
      logger.error('Error loading saved poems:', error)
      Pop.error(error?.response?.data || 'Failed to load saved poems')
      throw error
    }
  }
}

export const poemsService = new PoemsService()
