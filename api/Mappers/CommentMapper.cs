using api.Dtos.comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment CommentModel)
        {
            return new CommentDto
            {
                Id = CommentModel.Id,
                Title = CommentModel.Title, 
                Content = CommentModel.Content,
                CreatedOn = CommentModel.CreatedOn,
                CreatedBy = CommentModel.AppUser.UserName,
                StockId = CommentModel.StockId
            };
        }

        public static Comment ToCommandFromCreateCommentDto(this CreateCommentDto CreatedComment, int stockId)
        {
            return new Comment
            {
                Title = CreatedComment.Title,
                Content = CreatedComment.Content,
                CreatedOn = DateTime.Now,
                StockId = stockId
            };
        }
    }
}
