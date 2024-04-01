using JwtAuthenticationBackend.Data;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthenticationBackend.Controller
{
    [Route("login")]
    public class LoginController : ControllerBase
    {
        Database _database;
        public LoginController(Database database) {
            this._database = database;
        }



        [HttpGet]
        public string Get() {
            string message = "Users :\n";
            try
            {
                var query = _database.Users.Select(user => user);
                if(query.Count() > 0)
                {
                    foreach(var user in query)
                    {
                        message += "\'"+user.UserName + "\' ";
                    }
                }
                else
                {
                    message = "User not found ";
                }
                message += ".";
            }
            catch (Exception exp)
            {
                message = "Error Occurred, Contact Administrator.";
                Console.WriteLine(exp.Message);
            }
            return message;
        }


        [HttpPost] 
        public void Post() { }
    }
}
