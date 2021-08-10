using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Models.DTOs.ReactionTypes
{
    public class ReactionTypeConfirmationDto
    {
        /// <summary>
        /// Unique identifier of the reaction type
        /// </summary>
        public int ReactionTypeId { get; set; }

        /// <summary>
        /// Name of the reaction type
        /// </summary>
        public String TypeName { get; set; }
    }
}
