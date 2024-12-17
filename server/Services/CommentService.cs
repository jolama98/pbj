
namespace pbj.Services;

public class CommentService
{
    private readonly CommentRepository _commentRepository;

    public CommentService(CommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    internal List<Comment> GetAllComments()
    {
        List<Comment> comment = _commentRepository.GetAllComments();
        return comment;
    }
}
