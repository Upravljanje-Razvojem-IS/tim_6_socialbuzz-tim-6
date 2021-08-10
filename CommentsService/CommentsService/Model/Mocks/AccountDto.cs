using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentsService.Model.Mock
{
    public class AccountDto
    {
        /// <summary>
        /// Account ID
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public String Username { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Surname
        /// </summary>
        public String Surname { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        public String Phone_number { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public String Address { get; set; }

        /// <summary>
        /// Role ID
        /// </summary>
        public int RoleID { get; set; }
    }
}
