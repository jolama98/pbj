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
}
