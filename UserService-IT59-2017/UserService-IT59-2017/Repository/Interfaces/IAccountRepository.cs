using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService_IT59_2017.Models;

namespace UserService_IT59_2017.Repository.Interfaces
{
    public interface IAccountRepository
    {
        IEnumerable<PersonalAccount> GetAllPersonalAccounts();
        PersonalAccount GetAccountById(int id);
        void AddAccount(PersonalAccount account);
        void UpdateAccount(PersonalAccount account);
        void DeleteAccount(PersonalAccount account);
    }
}
