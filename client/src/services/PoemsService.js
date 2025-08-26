import { logger } from "@/utils/Logger.js"
import { api } from "./AxiosService.js"
import { Poem } from "@/models/Poem.js"
import { AppState } from "@/AppState.js"
import { LikedPoem } from "@/models/LikedPoem.js"
import Pop from "@/utils/Pop.js"

class PoemsService {

  async searchPoem(query) {
    try {
      const response = await api.get('api/poem/search', {
        params: { query: query || '' }
      });
      logger.log('Searched poems - poems service', response.data);
      const searchedPoems = response.data.map(poemData => new Poem(poemData));
      AppState.poems = searchedPoems;
    } catch (error) {
      logger.error('Error searching poems:', error);
      throw error;
    }
  }


  clearSearchQuery() {
    AppState.poemQuery = '';
    this.getAllPoems();
  }

  async getAllPoems() {
    const response = await api.get('api/poem')
    const newPoems = response.data.map(poemData => new Poem(poemData))
    AppState.poems = newPoems
  }

  async GetPoemById(poemId) {
    AppState.poemById = null
    const response = await api.get(`api/poem/${poemId}`)
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
  // async getAllSavedPoem() {
  //   const response = await api.get('api/savedPoems')
  //   const newPoems = response.data.map(savedPoemsData => new Poem(savedPoemsData))
  //   AppState.savedPoem = newPoems
  // }

}



export const poemsService = new PoemsService()
