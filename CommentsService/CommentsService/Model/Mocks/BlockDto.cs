using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentsService.Model.Mocks
{
    /// <summary>
    ///DTO block model
    /// </summary>
    public class BlockDto
    {
        /// <summary>
        /// ID of blocking user
        /// </summary>
        public int blockerID;

        /// <summary>
        /// ID of blocked user
        /// </summary>
        public int blockedID;
    }
}
