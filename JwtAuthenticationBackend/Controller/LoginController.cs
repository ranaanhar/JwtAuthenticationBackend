using JwtAuthenticationBackend.Data;
using JwtAuthenticationBackend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthenticationBackend.Controller
{
    [Route("login")]
    public class LoginController : ControllerBase
    {
        UserManager<IdentityUser> _usermanager; 
        public LoginController(UserManager<IdentityUser>userManager) {
           _usermanager = userManager;
        }



        [HttpGet]
        public void Get() {
           
        }


        [HttpPost] 
        public async Task<ActionResult<ResponseAuthentication>> Post([FromBody]RequestAuthentication request) {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request == null || string.IsNullOrEmpty(request.UsernameOrEmail) || string.IsNullOrEmpty(request.Password)) 
            {
                return BadRequest("bad credential.");
            }

            try
            {
                var user=await _usermanager.FindByNameAsync(request.UsernameOrEmail);

                if (user == null)
                {
                    user=await _usermanager.FindByEmailAsync(request.UsernameOrEmail);
                }


                if (user==null)
                {
                    return BadRequest("bad credential.");
                }

                var result=await _usermanager.CheckPasswordAsync(user,request.Password);
                if (!result)
                {
                    return BadRequest("bad credential.");
                }
                //todo
                return Ok(new ResponseAuthentication() { Expiration="now",Token="permit"});
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                return BadRequest("bad credential.");
            }

            //return Ok();
        }
    }
}
