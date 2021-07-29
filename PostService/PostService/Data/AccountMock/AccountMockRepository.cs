using PostService.Models.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.AccountMock
{
    public class AccountMockRepository : IAccountMockRepository
    {
        public static List<AccountDto> Accounts { get; set; } = new List<AccountDto>();

        public AccountMockRepository()
        {
            FillData();
        }

        private static void FillData()
        {
            Accounts.AddRange(new List<AccountDto>
            {
                new AccountDto
                {
                    AccountId = Guid.Parse("f2f88bcd-d0a2-4fe7-a23f-df97a59731cd"),
                    Username = "NinaK",
                    Name = "Nina",
                    Surname = "Kovacevic",
                    Email = "ninak@gmail.com",
                    Address = "Atinska 4",
                    PhoneNumber = "065223377"
                },
                new AccountDto
                {
                    AccountId = Guid.Parse("42b70088-9dbd-4b19-8fc7-16414e94a8a6"),
                    Username = "MarkoP",
                    Name = "Marko",
                    Surname = "Petrovic",
                    Email = "mare@gmail.com",
                    Address = "Nemanjina 45",
                    PhoneNumber = "065244557"
                },
                new AccountDto
                {
                    AccountId = Guid.Parse("59ed7d80-39c9-42b8-a822-70ddd295914a"),
                    Username = "DusanK",
                    Name = "Dusan",
                    Surname = "Krstic",
                    Email = "dusan98@gmail.com",
                    Address = "Niska 40",
                    PhoneNumber = "0647722398"
                }
            });
        }

        public AccountDto GetAccountByUsername(string username)
        {
            return Accounts.FirstOrDefault(e => e.Username == username);
        }
    }
}