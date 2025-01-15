import { logger } from "@/utils/Logger.js";
import { api } from "./AxiosService.js";
import Pop from "@/utils/Pop.js";
import { SavedPoem } from "@/models/SavedPoem.js";
import { AppState } from "@/AppState.js";

class SavePoemsService {

  async createSavePoem(createSavePoemData) {

    // console.log(createSavePoemData)

    const response = await api.post('api/savedpoem', createSavePoemData)
    logger.log("SAM I AM", response.data)
    const savedPoem = new SavedPoem(response.data)
    AppState.savedPoem.push(savedPoem)
    Pop.success('Poem In Book!')
  }
}

export const savePoemsService = new SavePoemsService;
