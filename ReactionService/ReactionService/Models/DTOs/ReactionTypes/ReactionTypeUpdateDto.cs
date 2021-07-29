using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Models.DTOs.ReactionTypes
{
    public class ReactionTypeUpdateDto
    {
        /// <summary>
        /// Name of the reaction type
        /// </summary>
        [Required]
        public String TypeName { get; set; }
    }
}
