using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService_IT59_2017.Models;

namespace UserService_IT59_2017.Repository.Interfaces
{
    public interface ICorporateAccount
    {
        IEnumerable<CorporateAccount> GetCorporateAccounts();
        CorporateAccount GetCorporateAccountById(int id);
        void AddCorporateAccount(CorporateAccount acc);
        void UpdateCorporateAccount(CorporateAccount acc);
        void DeleteCorporateAccount(CorporateAccount acc);
    }
}
