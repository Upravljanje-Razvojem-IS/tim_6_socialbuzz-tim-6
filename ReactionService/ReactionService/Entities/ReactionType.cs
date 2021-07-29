using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Entities
{
    /// <summary>
    /// Entity class which models different types of reactions
    /// </summary>
    public class ReactionType
    {
        /// <summary>
        /// Unique identifier of the reaction type
        /// </summary>
        [Key]
        [Required]
        public int ReactionTypeId { get; set; }

        /// <summary>
        /// Name of the reaction type
        /// </summary>
        [Required]
        public String TypeName{ get; set; }
    }
}
