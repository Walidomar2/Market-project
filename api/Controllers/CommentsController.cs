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
        private readonly IStockRepository _stockRepo;

        public CommentsController(ICommentRepository CommentRepo, IStockRepository stockRepo)
        {
            _commentRepo = CommentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var CommentsDto = comments.Select(c => c.ToCommentDto());

            return Ok(CommentsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);

            if(comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
       
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Create([FromRoute] int id, CreateCommentDto CreateComment)
        {

            if(! await _stockRepo.StockExists(id))
            {
                return BadRequest("Stock doesn't Exists");
            }

            var CommentModel = CreateComment.ToCommandFromCreateCommentDto(id);
            await _commentRepo.CreateAsync(CommentModel);
            
            return CreatedAtAction(nameof(GetById), new {id = CommentModel}, CommentModel.ToCommentDto());
        }
    }
}
