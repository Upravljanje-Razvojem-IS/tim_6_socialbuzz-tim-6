namespace BlockingService.Models.DTOs
{
    public class LoginDTO
    {
        /// <summary>
        /// Username of the user account who is trying to login
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password of the user account who is trying to login
        /// </summary>
        public string Password { get; set; }
    }
}
