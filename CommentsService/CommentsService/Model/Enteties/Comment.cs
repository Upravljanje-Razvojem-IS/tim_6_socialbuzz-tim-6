using CommentsService.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.Model.Enteties
{
    /// <summary>
    /// Comments model
    /// </summary>
    [Table("Comments")]
    public class Comment
    {

        /// <summary>
        /// Comment ID
        /// </summary>
        [Key]
        public Guid CommentID { get; set; }

        /// <summary>
        /// Post ID to which the comment refers
        /// </summary>
        public int PostID { get; set; }

        /// <summary>
        /// Account ID that posted comment
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// Account username that posted comment
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// Comment content
        /// </summary>
        [Required(ErrorMessage = "Comment text is required.")]
        public String CommentText { get; set; }

        /// <summary>
        /// Date when comment was posted
        /// </summary>
        [Required(ErrorMessage = "Date is required.")]
        public DateTime CommentDate { get; set; }
    }
}
