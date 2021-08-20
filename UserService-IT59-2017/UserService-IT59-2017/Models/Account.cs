using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserService_IT59_2017.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Address { get; set; }
        
        public virtual Role Rola { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public int RolaId { get; set; }
    }
}