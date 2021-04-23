using EvaluationsService.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationsService.Model.Enteties
{
    /// <summary>
    /// Evaluations model
    /// </summary>
    [Table("Evaluations")]
    public class Evaluation
    {
        /// <summary>
        /// Evaluation ID
        /// </summary>
        /// <example>e48da32e-f8d3-4b2e-aaff-2a3a4827188b</example>
        [Key]
        public Guid EvaluationID { get; set; }

        /// <summary>
        /// Evaluation mark
        /// </summary>
        /// <example>5</example>
        [Range(1, 5, ErrorMessage = "Mark must bee between 1 and 5")]
        [Required(ErrorMessage = "Mark is required.")]
        public int Mark { get; set; }

        /// <summary>
        /// Evaluation description
        /// </summary>
        /// <example>All went excelent, my recomendations for user.</example>
        [Required(ErrorMessage = "Description is required.")]
        public String Description { get; set; }

        /// <summary>
        /// Post ID to which the evaluation refers
        /// </summary>
        /// <example>1</example>
        public int PostID { get; set; }

        /// <summary>
        /// Account ID that submited evaluation
        /// </summary>
        /// <example>1</example>
        public int AccountID { get; set; }

        /// <summary>
        /// Account username that submited evaluation
        /// </summary>
        public Account Account { get; set; }
    }
}
