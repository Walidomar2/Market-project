using api.Data;
using api.Dtos.comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment?> CreateAsync(Comment CommentModel)
        {
            await _context.Comments.AddAsync(CommentModel);
            await _context.SaveChangesAsync();

            return CommentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var Comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (Comment == null)
            {
                return null;
            }

            _context.Comments.Remove(Comment);
            await _context.SaveChangesAsync();
            return Comment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }
    }
}
