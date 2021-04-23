using CommentsService.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.Model
{
    /// <summary>
    /// DTO comment model
    /// </summary>
    public class CommentingDto
    {
        /// <summary>
        /// Comment ID
        /// </summary>
        /// <example>e48da32e-f8d3-4b2e-aaff-2a3a4827188b</example>
        public Guid CommentID { get; set; }

        /// <summary>
        /// Post ID to which the comment refers
        /// </summary>
        /// <example>1</example>
        public int PostID { get; set; }

        /// <summary>
        /// Account ID that posted comment
        /// </summary>
        /// <example>1</example>
        public int AccountID { get; set; }

        /// <summary>
        /// Account username that posted comment
        /// </summary>
        public Account Username { get; }

        /// <summary>
        /// Comment content
        /// </summary>
        /// <example>Good stuff.</example>
        [Required(ErrorMessage = "Comment text is required.")]
        public String CommentText { get; set; }

        /// <summary>
        /// Date when comment was posted
        /// </summary>
        /// <example>2021-04-21T00:00:00</example>
        [Required(ErrorMessage = "Date is required.")]
        public DateTime CommentDate { get; set; }
    }
}
