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
