using Microsoft.AspNetCore.Identity;
using ProjectManagementSystem.Contracts;
using ProjectManagementSystem.Data.Models;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagementSystem.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentService(ApplicationDbContext _context, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;

        }

        /// <summary>
        /// Generates user comment in the opened "Task" page.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Adds new comment</returns>
        public async Task AddCommentAsync(CommentViewModel model)
        {

            var entity = new Comment()
            {
                AuthorId = model.AuthorId,
                CommentPost = model.CommentPost,
                SubtaskId = model.SubtaskId

            };

            await context.Comments.AddAsync(entity);
            await context.SaveChangesAsync();

        }

        /// <summary>
        /// Gets all comments from the DB
        /// </summary>
        /// <returns>All comments from the database as <IEnumerable></returns>
        public async Task<IEnumerable<CommentViewModel>> GetCommentsAsync()
        {

            var entities = await context.Comments
                .Include(c => c.Author)
                .Include(c => c.Subtask)
                .ToListAsync();

            return entities.Select(c => new CommentViewModel()
            {
                Id = c.Id,
                CommentPost = c.CommentPost,
                AuthorId = c.AuthorId,
                Author = c.Author.Name,
                SubtaskId = c.SubtaskId,
                Subtask = c.Subtask.Name
            });
        }

        /// <summary>
        /// Get and load the comments in the opened "Task" page.
        /// </summary>
        /// <param name="subtaskId"></param>
        /// <returns>The comments in the "Task" page.</returns>
        public async Task<IEnumerable<CommentViewModel>> GetCommentsByIdAsync(int subtaskId)
        {

            var entities = await context.Comments
                .Where(c => c.SubtaskId == subtaskId)
                .Include(c => c.Author)
                .Include(c => c.Subtask)
                .ToListAsync();

            return entities.Select(c => new CommentViewModel()
            {
                Id = c.Id,
                CommentPost = c.CommentPost,
                AuthorId = c.AuthorId,
                Author = c.Author.Name,
                SubtaskId = c.SubtaskId,
                Subtask = c.Subtask.Name
            });
        }

        /// <summary>
        /// Deletes comment entity from DB
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns>Finds the Comment in DB with ID = commentId and deletes it.</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task DeleteComment(int commentId)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                throw new ArgumentException("Comment not found");
            }

            context.Remove(comment);
            await context.SaveChangesAsync();

        }
    }
}
