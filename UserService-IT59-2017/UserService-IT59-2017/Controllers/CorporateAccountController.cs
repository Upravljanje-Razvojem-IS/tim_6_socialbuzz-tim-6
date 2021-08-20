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
    public class CorporateAccountController : ApiController
    {
        ICorporateAccount _repo;

        public CorporateAccountController()
        {
            _repo = new CorporateAccountRepository();
        }

        public IEnumerable<CorporateAccount> Get()
        {
            return _repo.GetCorporateAccounts();
        }

        public IHttpActionResult Get(int id)
        {
            CorporateAccount acc = _repo.GetCorporateAccountById(id);
            if (acc == null)
            {
                return NotFound();
            }
            return Ok(acc);
        }

        public IHttpActionResult Post([FromBody] CorporateAccount acc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.AddCorporateAccount(acc);
            return Ok();
        }

        public IHttpActionResult Put(int id, [FromBody] CorporateAccount acc)
        {
            if (id != acc.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.UpdateCorporateAccount(acc);
            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            CorporateAccount acc = _repo.GetCorporateAccountById(id);
            if (acc == null)
            {
                return NotFound();
            }
            _repo.DeleteCorporateAccount(acc);
            return Ok();
        }

    }
}
