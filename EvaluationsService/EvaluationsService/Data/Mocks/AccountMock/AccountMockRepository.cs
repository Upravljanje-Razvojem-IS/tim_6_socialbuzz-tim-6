using EvaluationsService.Model.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationsService.Data.Mocks.AccountMock
{
    public class AccountMockRepository : IAccountMockRepository
    {
        public static List<AccountDto> Accounts { get; set; } = new List<AccountDto>();

        public AccountMockRepository()
        {
            FillData();
        }

        private void FillData()
        {
            Accounts.AddRange(new List<AccountDto>
            {
                new AccountDto
                {
                    AccountID = 2,
                    Username = "flowzy1",
                    Name = "Darko",
                    Surname = "Mitrovic",
                    Email = "dareda@gmail.com",
                    Address = "Puskinova 14",
                    Phone_number = "066012534",
                    RoleID = 1
                },
                new AccountDto
                {
                    AccountID = 4,
                    Username = "zomag312",
                    Name = "Milica",
                    Surname = "Milanovic",
                    Email = "milicar@gmail.com",
                    Address = "Cara Dusana 15",
                    Phone_number = "064012534",
                    RoleID = 2
                }
            });
        }
        public String getUsernameByID(int ID)
        {
            var Acc = Accounts.Find(e => e.AccountID == ID);
            return Acc.Username;
        }

        public AccountDto GetAccountByID(int ID)
        {
            return Accounts.FirstOrDefault(e => e.AccountID == ID);
        }
    }
}
