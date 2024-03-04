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
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await _commentRepo.GetAllAsync();
            var CommentsDto = comments.Select(c => c.ToCommentDto());

            return Ok(CommentsDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.GetByIdAsync(id);

            if(comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
       
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> Create([FromRoute] int id, CreateCommentDto CreateComment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            if (! await _stockRepo.StockExists(id))
            {
                return BadRequest("Stock doesn't Exists");
            }

            var CommentModel = CreateComment.ToCommandFromCreateCommentDto(id);
            await _commentRepo.CreateAsync(CommentModel);
            
            return CreatedAtAction(nameof(GetById), new {id = CommentModel.Id}, CommentModel.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.DeleteAsync(id);
            if(comment == null)
            {
                return NotFound("Comment doesn't Exists To delete");
            }
            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateCommentDto UpdateComment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var UpdatedComment = await _commentRepo.UpdateAsync(id, UpdateComment);
            if(UpdatedComment == null)
            {
                return NotFound($"No Comment with id: {id} Found");
            }
            return Ok(UpdatedComment);
        }
    }
}
