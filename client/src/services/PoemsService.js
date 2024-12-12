import { logger } from "@/utils/Logger.js"
import { api } from "./AxiosService.js"

class PoemsService {
  async getAllPoems() {
    const response = await api.get('api/poem')
    logger.log('Got all poems - poems service', response.data)
  }


}

export const poemsService = new PoemsService()