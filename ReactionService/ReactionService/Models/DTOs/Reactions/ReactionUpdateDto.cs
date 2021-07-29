using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Models.DTOs.Reactions
{
    public class ReactionUpdateDto
    {
        /// <summary>
        /// Unique identifier of the reactionType 
        /// </summary>
        [Required]
        public int ReactionTypeId { get; set; }
    }
}
