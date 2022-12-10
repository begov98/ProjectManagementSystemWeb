using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Contracts;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controllers
{
    public class CommentController : Controller
    {
        private readonly ISubtaskService subtaskService;
        private readonly ICommentService commentService;
        public CommentController(ISubtaskService _subtaskService, ICommentService _commentService)
        {
            subtaskService = _subtaskService;
            commentService = _commentService;
        }

        /// <summary>
        /// Delete the comment found by it's ID.
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="subtaskId"></param>
        /// <returns>Redirects to the same "Task" page.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(int commentId, int subtaskId)
        {
            await commentService.DeleteComment(commentId);
            return RedirectToAction("Details", "Subtask", new { subtaskId = subtaskId });
        }
    }
}
