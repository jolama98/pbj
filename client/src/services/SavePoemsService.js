import { logger } from "@/utils/Logger.js";
import { api } from "./AxiosService.js";

class SavePoemsService {
  async createSavePoem(value) {
    const response = await api.post('api/savedpoem', value)
    logger.log(response.data)
  }
}

export const savePoemsService = new SavePoemsService;
