using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowingService.Models.DTOs
{
    public class FollowUserDTO
    {
        /// <summary>
        /// Account Id of the user that will be followed/unfollowed
        /// </summary>
        public string Following{ get; set; }
    }
}
