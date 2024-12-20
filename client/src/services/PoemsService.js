import { logger } from "@/utils/Logger.js"
import { api } from "./AxiosService.js"
import { Poem } from "@/models/Poem.js"
import { AppState } from "@/AppState.js"

class PoemsService {
  async getAllPoems() {
    const response = await api.get('api/poem')
    logger.log('Got all poems - poems service', response.data)
    const newPoems = response.data.map(poemData => new Poem(poemData))
    AppState.poems = newPoems
  }
  async GetPoemById(poemId) {
    AppState.poemById = null
    const response = await api.get(`api/poem/${poemId}`)
    logger.log(response.data)
    AppState.poemById = response.data
  }
  async deletePoem(poemId) {
    const response = await api.delete(`api/poem/${poemId}`)
    logger.log('DELETED POEM 💥', response.data)
    const poemData = AppState.poems.findIndex(PoemToDelete => PoemToDelete.id == poemId)
    AppState.poems.splice(poemData, 1)
  }

  async createPoem(poemData) {
    const response = await api.post('api/poem', poemData)
    const newPoem = new Poem(response.data)
    AppState.poems.unshift(newPoem)
  }

}

export const poemsService = new PoemsService()
