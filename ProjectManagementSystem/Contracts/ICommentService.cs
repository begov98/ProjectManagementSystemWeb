using ProjectManagementSystem.Data.Models;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Contracts
{
    public interface ICommentService
    {
        Task AddCommentAsync(CommentViewModel model);

        Task<IEnumerable<CommentViewModel>> GetCommentsAsync();

        Task<IEnumerable<CommentViewModel>> GetCommentsByIdAsync(int subtaskId);

        Task DeleteComment(int commentId);

    }
}
