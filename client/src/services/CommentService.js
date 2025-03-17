import { logger } from "@/utils/Logger.js"
import { api } from "./AxiosService.js"
import Pop from "@/utils/Pop.js"
import { AppState } from "@/AppState.js"
import { Comment } from "@/models/Comments.js"

class CommentService {

  async createComment(commentData, poemId) {
    const response = await api.post('api/comment', commentData)
    logger.log('Comment data', response.data, poemId)
    const newComment = new Comment(response.data)
    AppState.comments.unshift(newComment)
    Pop.success("Comment created!")
  }

  async getAllCommentsByPoemId(poemId) {
    AppState.activePoemComments = []
    const response = await api.get(`api/poem/${poemId}/comments`)
    logger.log('📈📈📈📈', response.data)
  }




  // async getCommentsByPostId(postId) {
  //   AppState.activePostComments = []
  //   const response = await api.get(`api/posts/${postId}/comments`)
  //   logger.log('📈📈📈📈', response.data)
  //   const activePostComments = response.data.map(commentPOJO => new Comment(commentPOJO))
  //   AppState.activePostComments = activePostComments
  // }
}
export const commentsService = new CommentService()
