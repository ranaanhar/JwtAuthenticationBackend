using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthenticationBackend.Controller;

//[ApiController]
[EnableCors("cors")]
[Route("usermanager")]
public class UserManagerController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<UserManagerController> _logger;
    //private readonly SignInManager<IdentityUser> _signInManager;

    public UserManagerController(UserManager<IdentityUser> userManager, ILogger<UserManagerController> logger)
    {//},SignInManager<IdentityUser>signInManager){
        _userManager = userManager;
        _logger = logger;
        //_signInManager = signInManager;
    }

    [Route("deleteuser")]
    [Authorize(Roles = Data.DataCostants.Admin_Role + "," + Data.DataCostants.User_Role)]
    [HttpPost]
    public ActionResult deleteUser(string UserName)
    {
        //additional Authorization
        var role = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).First();
        if (!string.IsNullOrEmpty(role))
        {
            if (role == Data.DataCostants.User_Role)
            {
                var userName = User.Claims.Where(c => c.Type == ClaimTypes.Name).Select(r => r.Value).First();
                if (!string.IsNullOrEmpty(userName) && userName == UserName)
                {
                    deleteAccount(userName);
                    return Ok("user removed.");
                }
            }
            else if (role == Data.DataCostants.Admin_Role)
            {
                deleteAccount(UserName);
                return Ok("user removed.");
            }
        }
        return BadRequest("user not found.");
    }

    private void deleteAccount(string UserName)
    {
        if (!string.IsNullOrEmpty(UserName))
        {
            var user = _userManager.Users.Where(u => u.UserName == UserName).First();
            if (user != null)
            {
                _userManager.DeleteAsync(user);

            }
        }
    }


    // [Route("getuser")]
    // [Authorize(Roles = Data.DataCostants.Admin_Role)]
    // [HttpPost]
    // public void GetUser() { }


    [Route("getall")]
    [Authorize(Roles = Data.DataCostants.Admin_Role)]
    [HttpPost]
    public ActionResult<IEnumerable<string>> GetAllUser(string id)
    {
        try
        {
            var result = _userManager.Users.Select(x => x.UserName).AsEnumerable();
            _logger.LogInformation("Get All User");
            return Ok(result);
        }
        catch (Exception exp)
        {
            _logger.LogError(exp, "Crud");
        }
        return BadRequest("couldn't return user.");
    }


    [Route("updateuser")]
    [Authorize(Roles = Data.DataCostants.User_Role)]
    [HttpPost]
    public async Task<ActionResult> UpdateUser([FromBody] IdentityUser user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Enter Valid user.");
        }

        var _user = await _userManager.FindByNameAsync(user.UserName!);

        if (_user != null)
        {
            _user.PhoneNumber = user.PhoneNumber;
            _user.Email = user.Email;

            //TODO check the database
            _user.UserName = user.UserName;

            var result = await _userManager.UpdateAsync(_user);
            if (result.Succeeded)
            {
                return Ok("user updated");
            }
        }
        return BadRequest("error");
    }

}
