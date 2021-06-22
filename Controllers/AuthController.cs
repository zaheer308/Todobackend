using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoBackend.Data;
using TodoBackend.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository auth;
        private readonly DbContextBase _context;


        public AuthController(IAuthRepository auth, DbContextBase context)
        {

            this.auth = auth;
            _context = context;

        }
        [HttpPost]
        [Route("[action]")]
        public async Task<object> login([FromBody] LoginData model)
        {
            var username = new string(model.username);
            var password = new string(model.password);
            var k = await this.auth.Login(username, password);

            return k;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<object> Register([FromBody] LoginData model)
        {
            var username = new string(model.username);
            var password = new string(model.password);
            var ki = await this.auth.UserExists(username);
            if (ki == true)
            {
                return Ok("User Already Exist");
            }
            else
            {


                var k = await this.auth.Register(username, password);

                return k;
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult verifyEmail([FromBody] VerifyModel obj)
        {
            var verification = Convert.ToInt32(obj.Verification);
            var username = obj.Username;
            User item = _context.Users.Where(f => f.Username == username && f.Verification == verification).FirstOrDefault();
            if (item == null)
            {
                return Ok("Wrong Code!");
            }

            item.IsVerified = 1;

            _context.Users.Update(item);
            _context.SaveChanges();

            return Ok("Email Verified!");
        }



    }

}
