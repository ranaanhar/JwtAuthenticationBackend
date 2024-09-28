using JwtAuthenticationBackend.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthenticationBackend.Controller
{
    [EnableCors("cors")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly ILogger<RegistrationController> _logger;
        readonly RoleManager<IdentityRole> _roleManager;
        public RegistrationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<RegistrationController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        public void Get() {
            throw new NotImplementedException();
         }

        [HttpPost]
        public async Task<ActionResult<ResponseSignup>> Post([FromBody] RequestSignup request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request == null ||
                string.IsNullOrEmpty(request.Username) ||
                string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(nameof(request));
            }

            try
            {
                var exist = await _userManager.FindByNameAsync(request.Username);
                if (exist == null)
                {
                    exist = await _userManager.FindByEmailAsync(request.Email);
                }

                if (exist != null)
                {
                    _logger.LogInformation(string.Format("user already exists."));
                    return BadRequest("fields error");
                }


                //create user instance
                var user = new ApplicationUser()
                {
                    UserName = request.Username,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber
                };


                //add user to database
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    var response = new ResponseSignup(user);
                    if (!string.IsNullOrEmpty(response.Username))
                    {
                        //add user to role
                        await addUserToRole(user);
                        _logger.LogInformation(string.Format("user {0} registered.", response));
                        return Ok(response);
                    }
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        _logger.LogInformation(item.Description);
                    }
                }
            }
            catch (Exception exp)
            {
                _logger.LogInformation(exp.Message);
                return BadRequest("fileds error.");
            }

            return BadRequest("fileds error.");
        }

        //add user to role
        private async Task addUserToRole(ApplicationUser user)
        {
            var role = _roleManager.FindByNameAsync(Data.DataConstants.User).Result;
            if (role != null)
            {
                await _userManager.AddToRoleAsync(user, Data.DataConstants.User);
            }
        }
    }
}
