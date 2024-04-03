using JwtAuthenticationBackend.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthenticationBackend.Controller
{
    [EnableCors("cors")]
    [Route("register")]
    public class RegistrationController : ControllerBase
    {
        UserManager<IdentityUser> _userManager;
        ILogger<RegistrationController> _logger;
        public RegistrationController(UserManager<IdentityUser> userManager, ILogger<RegistrationController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public void Get() { }

        [HttpPost]
        public async Task<ActionResult<ResponseSignup>> Post([FromBody]RequestSignup request) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState); }

            if (request == null ||
                string.IsNullOrEmpty(request.Username) ||
                string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("fileds error.");
            }

            try
            {
                var exist =await _userManager.FindByNameAsync(request.Username);
                if (exist == null) {
                    exist = await _userManager.FindByEmailAsync(request.Email);
                }
                
                if(exist!=null) {
                    _logger.LogInformation(string.Format("user already exists."));
                    return BadRequest("fields error");
                }


                var user = new IdentityUser() {
                    UserName=request.Username,
                    Email=request.Email,
                    PhoneNumber=request.PhoneNumber
                };


                var result =await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    var response = new ResponseSignup(user);
                    if (!string.IsNullOrEmpty(response.Username))
                    {
                        _logger.LogInformation(string.Format("user {0} registered.",response));
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
    }
}
