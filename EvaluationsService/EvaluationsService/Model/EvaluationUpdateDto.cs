using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationsService.Model
{
    public class EvaluationUpdateDto
    {
        /// <summary>
        /// Evaluation ID
        /// </summary>
        /// <example>e48da32e-f8d3-4b2e-aaff-2a3a4827188b</example>
        [Required(ErrorMessage = "Evaluation ID is required")]
        public Guid EvaluationID { get; set; }

        /// <summary>
        /// Evaluation mark
        /// </summary>
        /// <example>2</example>
        [Required(ErrorMessage = "Evaluation mark is required")]
        [Range(1, 5, ErrorMessage = "Mark must bee between 1 and 5")]
        public int Mark { get; set; }

        /// <summary>
        /// Evaluation description
        /// </summary>
        /// <example>I don't recommend this user.</example>
        [Required(ErrorMessage = "Description is required")]
        public String Description { get; set; }
    }
}
