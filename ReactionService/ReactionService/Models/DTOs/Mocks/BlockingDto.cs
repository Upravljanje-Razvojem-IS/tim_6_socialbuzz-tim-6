using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Models.DTOs.Mocks
{
    public class BlockingDto
    {
        /// <summary>
        /// Unique identifier for the blocking
        /// </summary>
        public Guid BlockingId;

        /// <summary>
        /// Id of the user who blocked another user
        /// </summary>
        public Guid BlockerId;

        /// <summary>
        /// Id of the user who are blocked by another user
        /// </summary>
        public Guid BlockedId;
    }
}
