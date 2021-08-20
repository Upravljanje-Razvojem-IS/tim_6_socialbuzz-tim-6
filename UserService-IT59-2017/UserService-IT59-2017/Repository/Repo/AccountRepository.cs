using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using UserService_IT59_2017.Models;
using UserService_IT59_2017.Repository.Interfaces;

namespace UserService_IT59_2017.Repository.Repo
{
    public class AccountRepository : IAccountRepository
    {
        private ApplicationDbContext db;

        public AccountRepository()
        {
            db = new ApplicationDbContext();
        }

        public IEnumerable<PersonalAccount> GetAllPersonalAccounts()
        {
            List<PersonalAccount> accs = db.PersonalAccounts.ToList();
            foreach(PersonalAccount ac in accs)
            {
                ac.Rola.Accounts = null;
            }
            return accs;
        }

        public PersonalAccount GetAccountById(int id)
        {
            return db.PersonalAccounts.Find(id);
        }

        public void AddAccount(PersonalAccount account)
        {
            db.PersonalAccounts.Add(account);
            db.SaveChangesAsync();
        }

        public void UpdateAccount(PersonalAccount account)
        {
            try
            {
                db.Entry<PersonalAccount>(account).State = System.Data.Entity.EntityState.Modified;
                db.SaveChangesAsync();
            }
            catch (DBConcurrencyException e)
            {
                Trace.TraceInformation(e.Message);
            }
        }

        public void DeleteAccount(PersonalAccount account)
        {
            db.PersonalAccounts.Remove(account);
            db.SaveChangesAsync();
        }

    }
}