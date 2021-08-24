using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserService.Entities
{
    public class Role
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Role name
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// List of accounts
        /// </summary>
        public List<Account> Accounts { get; set; }
    }
}
