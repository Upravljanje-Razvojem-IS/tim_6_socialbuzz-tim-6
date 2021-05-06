﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Value;

namespace EvaluationsService.Model.ValueObjects
{
    /// <summary>
    /// Account Value Object
    /// </summary>
    public class Account : ValueObject<Account>
    {
        /// <summary>
        /// Account username that submited evaluation
        /// </summary>
        /// <example>flowzy1</example>
        [Required(ErrorMessage = "Account username is required.")]
        public string Username { get; set; }

        private Account()
        {

        }
        public Account(string username)
        {
            Username = username;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Username;
        }
    }
}