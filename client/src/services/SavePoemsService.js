import { logger } from "@/utils/Logger.js";
import { api } from "./AxiosService.js";
import Pop from "@/utils/Pop.js";

class SavePoemsService {

  async createSavePoem(createSavePoemData) {
    const response = await api.post('api/savedpoem', createSavePoemData)
    logger.log("WHY", createSavePoemData)
    logger.log("SAM I AM", response.data)
    Pop.success('Poem In Book!')
  }
}

export const savePoemsService = new SavePoemsService;
