using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Entities
{
    /// <summary>
    /// Entity class which models reactions which are added to the posts
    /// </summary>
    public class Reaction
    {
        /// <summary>
        /// Unique identifier of the reaction
        /// </summary>
        [Key]
        [Required]
        public Guid ReactionId { get; set; }

        /// <summary>
        /// PostId to which the reaction is added
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// Unique identifier of the reaction type
        /// </summary>
        public int ReactionTypeId { get; set; }

        /// <summary>
        /// Id of the user who adds reaction
        /// </summary>
        public Guid AccountId { get; set; }

        public virtual ReactionType ReactionType { get; set; }
    }
}
