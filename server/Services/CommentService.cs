namespace pbj.Services;

public class CommentService
{
    private readonly CommentRepository _commentRepository;

    public CommentService(CommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    internal Comment CreateAComment(Comment commentData)
    {
        Comment comment = _commentRepository.CreateAComment(commentData);
        return comment;
    }

    internal string DestroyComment(int commentId, string userId)
    {
        Comment comment = GetCommentById(commentId);
        if (comment.CreatorId != userId)
        {
            throw new Exception("YOU DID NOT CREATE THIS COMMENT! GET AWAY!!!!!");
        }
        _commentRepository.DestroyComment(commentId);
        return "Comment was DELETED";
    }

    internal List<Comment> GetAllComments()
    {
        List<Comment> comment = _commentRepository.GetAllComments();
        return comment;
    }

    internal Comment GetCommentById(int commentId)
    {
        Comment comment = _commentRepository.GetCommentById(commentId) ?? throw new Exception($"No Comment found with this id of {commentId}");
        return comment;
    }

    internal Comment UpdateComment(int commentId, string userId, Comment commentData)
    {
        Comment commentToUpdate = GetCommentById(commentId);
        if (commentToUpdate.CreatorId != userId)
        {
            throw new Exception("YOU CANNOT UPDATE A COMMENT YOU DID NOT CREATE, THAT IS FORBIDDEN, PLEASE IGNORE THE 400 ERROR CODE, IT SHOULD BE 403");
        }

        commentToUpdate.Title = commentData.Title ?? commentData.Title;
        commentToUpdate.Body = commentData.Body ?? commentData.Body;
        _commentRepository.UpdateComment(commentToUpdate);
        return commentToUpdate;
    }

}
