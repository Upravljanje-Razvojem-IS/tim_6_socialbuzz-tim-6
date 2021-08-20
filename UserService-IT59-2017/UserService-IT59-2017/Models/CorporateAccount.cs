using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserService_IT59_2017.Models
{
    public class CorporateAccount : Account
    {
        [Required(ErrorMessage = "Corporation name is required")]
        public string CorporationName { get; set; }
    }
}