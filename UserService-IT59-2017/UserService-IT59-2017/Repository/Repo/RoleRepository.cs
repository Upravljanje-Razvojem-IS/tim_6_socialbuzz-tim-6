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
    public class RoleRepository : IRoleRepository
    {
        private ApplicationDbContext db;

        public RoleRepository()
        {
            db = new ApplicationDbContext();
        }

        public IEnumerable<Role> GetAllRoles()
        {
            List<Role> roles = db.Roles.ToList();
            foreach(Role r in roles)
            {
                r.Accounts = null;
            }
            return roles;
        }

        public Role GetRoleById(int id)
        {
            return db.Roles.Find(id);
        }

        public void AddRole(Role role)
        {
            db.Roles.Add(role);
            db.SaveChangesAsync();
        }

        public void UpdateRole(Role role)
        {
            try
            {
                db.Entry<Role>(role).State = System.Data.Entity.EntityState.Modified;
                db.SaveChangesAsync();
            }
            catch(DBConcurrencyException e)
            {
                Trace.TraceInformation(e.Message);
            }
            
        }

        public void DeleteRole(Role role)
        {
            db.Roles.Remove(role);
            db.SaveChanges();
        }
    }
}