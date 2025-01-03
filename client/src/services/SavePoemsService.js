import { logger } from "@/utils/Logger.js";
import { api } from "./AxiosService.js";
import Pop from "@/utils/Pop.js";

class SavePoemsService {
  async createSavePoem(createSavePoemData) {
    logger.log("See dog go")
    const response = await api.post('api/SavedPoem', createSavePoemData)
    logger.log("SAM I AM", response.data)
    Pop.success('Poem In Book!')
  }
}

export const savePoemsService = new SavePoemsService;
