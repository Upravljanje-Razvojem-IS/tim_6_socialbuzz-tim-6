using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserService_IT59_2017.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        public string Role_Name { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}