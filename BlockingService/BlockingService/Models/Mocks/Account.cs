using System;

namespace BlockingService.Models.Mocks
{
    /// <summary>
    /// Account DTO
    /// </summary>
    public class Account
    {

        /// <summary>
        /// Account ID
        /// </summary>
        public Guid Account_id { get; set; }
        /// <summary>
        /// Username of the user account
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Name of the user 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Surname of the user
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Password of the user account
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Generated JWT for authorization
        /// </summary>
        public string JWT { get; set; }
        public Account()
        {

        }
        public Account(string username, string password)
        {
            Username = username;
            Password = password;
        }


    }
}
