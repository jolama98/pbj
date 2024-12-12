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


}

export const poemsService = new PoemsService()