using System;

namespace BlockingService.Models.DTOs
{
    public class BlockingDTO
    {

        /// <summary>
        /// Blocker ID
        /// </summary>
        public Guid BlockerId { get; set; }

        /// <summary>
        /// Blocked ID
        /// </summary>
        public Guid BlockedId { get; set; }

    }
}
