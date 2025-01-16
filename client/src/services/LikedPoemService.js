import { LikedPoem } from "@/models/LikedPoem.js"
import { api } from "./AxiosService.js"
import { AppState } from "@/AppState.js"
import { logger } from "@/utils/Logger.js"
import Pop from "@/utils/Pop.js"

class LikedPoemService {
  async createLikePoem(poemId) {
    const response = await api.post('api/likedpoem', poemId)
    logger.log("I AM LIKED I AM", response.data)
    const likedPoem = new LikedPoem(response.data)
    AppState.likedPoem.push(likedPoem)
    Pop.success('POEM IS LIKED!')

  }
}

export const likedPoemService = new LikedPoemService;
