using System;
using System.ComponentModel.DataAnnotations;

namespace BlockingService.Model.Entity
{
    /// <summary>
    /// Blocking model
    /// </summary>
    public class Blocking
    {
        /// <summary>
        /// Blocker ID
        /// </summary>
        [Key]
        public Guid BlockerId { get; set; }
        /// <summary>
        /// Blocked ID
        /// </summary>
        [Key]
        public Guid BlockedId { get; set; }

    

    }
}
