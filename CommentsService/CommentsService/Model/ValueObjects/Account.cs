using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Value;
using ValueOf;

namespace CommentsService.Model.ValueObjects
{
    public class Account : ValueObject<Account>
    {
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
