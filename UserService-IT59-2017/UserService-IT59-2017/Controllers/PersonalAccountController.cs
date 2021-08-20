using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserService_IT59_2017.Models;
using UserService_IT59_2017.Repository.Interfaces;
using UserService_IT59_2017.Repository.Repo;

namespace UserService_IT59_2017.Controllers
{
    public class PersonalAccountController : ApiController
    {
        IAccountRepository _repo;

        public PersonalAccountController()
        {
            _repo = new AccountRepository();
        }

        public IEnumerable<PersonalAccount> Get()
        {
            return _repo.GetAllPersonalAccounts();
        }

        public IHttpActionResult Get(int id)
        {
            PersonalAccount acc = _repo.GetAccountById(id);
            if (acc == null)
            {
                return NotFound();
            }
            return Ok(acc);
        }

        public IHttpActionResult Post([FromBody] PersonalAccount acc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repo.AddAccount(acc);
            return Ok();
        }

        public IHttpActionResult Put(int id, [FromBody] PersonalAccount acc)
        {
            if (id != acc.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.UpdateAccount(acc);
            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            PersonalAccount acc = _repo.GetAccountById(id);
            if (acc == null)
            {
                return NotFound();
            }
            _repo.DeleteAccount(acc);
            return Ok();
        }

    }
}
