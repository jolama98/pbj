import { logger } from "@/utils/Logger.js"
import { api } from "./AxiosService.js"
import Pop from "@/utils/Pop.js"

class CommentService {

  async createComment(commentData, poemId) {
    const response = await api.post('api/comment', commentData)
    logger.log('Comment data', response.data, poemId)
    Pop.success("Comment created!")
  }
}
export const commentsService = new CommentService()
