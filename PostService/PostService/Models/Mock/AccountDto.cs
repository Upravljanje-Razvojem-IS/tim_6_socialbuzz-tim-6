using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models.Mock
{
    public class AccountDto
    {
        /// <summary>
        /// Unique identifier for the user account
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Username for the user account
        /// </summary>
        public String Username { get; set; }

        /// <summary>
        /// First name of the user
        /// is active
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Last name of the user
        /// is active
        /// </summary>
        public String Surname { get; set; }

        /// <summary>
        /// Email of the user
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// User's phone number
        /// </summary>
        public String PhoneNumber { get; set; }

        /// <summary>
        /// User's home address
        /// </summary>
        public String Address { get; set; }
    }
}
