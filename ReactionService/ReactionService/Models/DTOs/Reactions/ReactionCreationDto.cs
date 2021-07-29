using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Models.DTOs.Reactions
{
    public class ReactionCreationDto
    {
        /// <summary>
        /// Unique identifier of the post to which the reaction is added
        /// </summary>
        [Required]
        public Guid PostId { get; set; }

        /// <summary>
        /// Unique identifier of the reactionType 
        /// </summary>
        [Required]
        public int ReactionTypeId { get; set; }
    }
}
