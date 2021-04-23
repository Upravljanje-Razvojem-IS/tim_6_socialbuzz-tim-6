using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.Model
{
    /// <summary>
    /// DTO for comment creation
    /// </summary>
    public class CommentingCreateDto
    {
        /// <summary>
        /// Post ID to which the comment was added
        /// </summary>
        public int PostID { get; set; }

        /// <summary>
        /// Comment content
        /// </summary>
        [Required(ErrorMessage = "Comment text is required!")]
        public String CommentText { get; set; }

    }
}
