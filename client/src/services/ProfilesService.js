import { logger } from "@/utils/Logger.js"
import { api } from "./AxiosService.js"
import { Profile } from "@/models/Profile.js"
import { AppState } from "@/AppState.js"

class ProfilesService {
  async getProfileById(profileId) {
    AppState.activeProfile = null
    const response = await api.get(`api/profiles/${profileId}`)
    // logger.log('Got profile - profiles service', response.data)
    const newProfile = new Profile(response.data)
    AppState.activeProfile = newProfile
  }
}

export const profilesService = new ProfilesService()
