using Microsoft.AspNetCore.Mvc;
using Server.ClientBuisnessLogicLayer;
using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService service;

        public UserController(IUserService service)
        {
            this.service = service;
        }
         
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return service.SelectAll();
        }
                 
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return service.SelectById(id);
        }

        [HttpPost]
        public void Post([FromBody] User user)
        {
            service.Insert(user);
        }
 
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User user)
        {
            user.UserID = id;
            service.Update(user);
        }
 
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            User user = service.SelectById(id);
            service.Delete(user);
        }
    }
}
