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
        private int blockerID;
        public int BlockerID
        {
            get { return blockerID; }
            set { blockerID = value; }
        }

        /// <summary>
        /// ID of blocked user
        /// </summary>
        private int blockedID;
        public int BlockedID
        {
            get { return blockedID; }
            set { blockedID = value; }
        }
    }
}
