using System.ComponentModel.DataAnnotations;

namespace api.Dtos.comment
{
    public class UpdateCommentDto
    {

        [Required]
        [MinLength(5, ErrorMessage = "Title must be 5 characters at least")]
        [MaxLength(100, ErrorMessage = "Title maximum length 100 characters")]
        public string Title { get; set; } = string.Empty;


        [Required]
        [MinLength(5, ErrorMessage = "content must be 5 characters at least")]
        [MaxLength(250, ErrorMessage = "content maximum length 250 characters")]
        public string Content { get; set; } = string.Empty;
    }
}
