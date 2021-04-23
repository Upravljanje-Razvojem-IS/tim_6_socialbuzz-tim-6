using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationsService.Model
{
    public class EvaluationCreateDto
    {
        /// <summary>
        /// Evaluation mark
        /// </summary>
        [Required(ErrorMessage = "Evaluation mark is required")]
        [Range(1, 5, ErrorMessage = "Mark must bee between 1 and 5")]
        public int Mark { get; set; }

        /// <summary>
        /// Evaluation description
        /// </summary>
        [Required(ErrorMessage = "Description is required")]
        public String Description { get; set; }

        /// <summary>
        /// Post ID to which the evaluation refers
        /// </summary>
        public int PostID { get; set; }
    }
}
