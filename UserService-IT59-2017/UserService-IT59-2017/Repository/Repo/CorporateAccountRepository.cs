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
    public class CorporateAccountRepository : ICorporateAccount
    {
        ApplicationDbContext db;

        public CorporateAccountRepository()
        {
            db = new ApplicationDbContext();
        }

        public IEnumerable<CorporateAccount> GetCorporateAccounts()
        {
            List<CorporateAccount> accs = db.CorporateAccounts.ToList();
            foreach(CorporateAccount a in accs)
            {
                a.Rola.Accounts = null;
            }
            return accs;
        }

        public CorporateAccount GetCorporateAccountById(int id)
        {
            CorporateAccount acc = db.CorporateAccounts.Find(id);
            if (acc != null)
            {
                acc.Rola = db.Roles.Find(acc.RolaId);
                acc.Rola.Accounts = null;
            }
            return acc;
        }

        public void AddCorporateAccount(CorporateAccount acc)
        {
            db.CorporateAccounts.Add(acc);
            db.SaveChangesAsync();
        }

        public void UpdateCorporateAccount(CorporateAccount acc)
        {
            try
            {
                db.Entry<CorporateAccount>(acc).State = System.Data.Entity.EntityState.Modified;
                db.SaveChangesAsync();
            }
            catch (DBConcurrencyException e)
            {
                Trace.TraceInformation(e.Message);
            }
        }

        public void DeleteCorporateAccount(CorporateAccount acc)
        {
            db.CorporateAccounts.Remove(acc);
            db.SaveChangesAsync();
        }

    }
}