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
        private Guid blockingId;

        public Guid BlockingId
        {
            get { return BlockingId; }
            set { BlockingId = value; }
        }

        /// <summary>
        /// Id of the user who blocked another user
        /// </summary>
        private Guid blockerId;

        public Guid BlockerId
        {
            get { return BlockerId; }
            set { BlockerId = value; }
        }

        /// <summary>
        /// Id of the user who are blocked by another user
        /// </summary>
        private Guid blockedId;

        public Guid BlockedId
        {
            get { return BlockedId; }
            set { BlockedId = value; }
        }
    }
}
