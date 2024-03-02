using api.Dtos.comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;

        public CommentsController(ICommentRepository CommentRepo)
        {
            _commentRepo = CommentRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var CommentsDto = comments.Select(c => c.ToCommentDto());

            return Ok(CommentsDto);
        }
    }
}
