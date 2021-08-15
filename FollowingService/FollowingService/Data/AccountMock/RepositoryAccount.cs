using FollowingService.Models.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FollowingService.Data.AccountMock
{
    public class RepositoryAccount : IRepositoryAccount
    {
        public static List<Account> Accounts { get; set; } = new List<Account>();

        public RepositoryAccount()
        {
            SeedData();
        }

        private void SeedData()
        {
            Accounts.AddRange(new List<Account>
            {
                new Account
                {
                    Account_id = Guid.Parse("eb78bf24-a3ab-48bf-a931-7b0c6e680bfd"),
                    Username = "jovana",
                    Name = "Jovana",
                    Surname = "Bojicic",
                    Password = "jovanaa"

                },
                new Account
                {
                    Account_id = Guid.Parse("34a81ef8-2831-4444-8355-859d02ae2290"),
                    Username = "marko",
                    Name = "Marko",
                    Surname = "Markovic",
                    Password = "markoo"
                },
                new Account
                {
                    Account_id = Guid.Parse("7b585f96-e6df-481b-a079-64acef59bb9b"),
                    Username = "andrijana",
                    Name = "Andrijana",
                    Surname = "Milovanovic",
                    Password = "andrijanaa"

                }
            });
        }

        public  Account GetAccountByUserName(string username)
        {
            return Accounts.First(e => e.Username == username);
        }

        public  Account Exists(Account account)
        {
            return Accounts.First(a => a.Username == account.Username && a.Password == account.Password);
        }
    }
}
