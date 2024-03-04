using api.Dtos.comment;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        public Task<List<Comment>> GetAllAsync();
        public Task<Comment?> GetByIdAsync(int id);
        public Task<Comment?> CreateAsync(Comment CommentModel);
        public Task<Comment?> DeleteAsync(int id);
        public Task<Comment?> UpdateAsync(int id,UpdateCommentDto CommentModel);
    }
}
