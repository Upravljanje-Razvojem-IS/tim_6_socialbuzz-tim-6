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
    public class RolesController : ApiController
    {
        IRoleRepository _repo;

        public RolesController()
        {
            _repo = new RoleRepository();
        }

        public IEnumerable<Role> Get()
        {
            return _repo.GetAllRoles();
        }

        [Authorize]
        public IHttpActionResult Get(int id)
        {
            Role r = _repo.GetRoleById(id);
            if (r == null)
            {
                return NotFound();
            }
            return Ok(r);
        }

        [Authorize]
        public IHttpActionResult Post([FromBody] Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.AddRole(role);
            return Ok();
        }

        [Authorize]
        public IHttpActionResult Put(int id, [FromBody] Role role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repo.UpdateRole(role);
            return Ok();
        }

        [Authorize]
        public IHttpActionResult Delete(int id)
        {
            Role r = _repo.GetRoleById(id);
            if (r == null)
            {
                return NotFound();
            }
            _repo.DeleteRole(r);
            return Ok();
        }

    }
}
