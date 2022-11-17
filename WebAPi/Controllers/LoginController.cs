using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPi.Models;
//For more /*https://www.akana.com/blog/what-is-jwt*/
namespace WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private webapiContext _webapicontext;
        public LoginController(IConfiguration config)//IConfiguration interface is used to read Settings and Connection Strings from AppSettings.
        {
            _config = config;
        }
        
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)//This method is used for login authentication, and I use the userlogin model because I only used the username and password when using login.  
        {
            var user = Authenticate(userLogin);//This method return user data if exists 

            if (user != null)
            {
                var token = Generate(user);//Used to generate token
                return Ok(token);
            }

            return NotFound("User not found");
        }

        private string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));//Have to learn
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);//Signing credentials are used to protect against tampering.

            var claims = new[]//Using both the claim type and claim value in policy based authorization
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),// Claims are used to transmit information between two parties.What these claims are depends on the use case at hand. For example, a claim may assert who issued the token, how long it is valid for, or what permissions the client has been granted.
                new Claim(ClaimTypes.Email, user.EmailAddress),   //
                new Claim(ClaimTypes.GivenName, user.GivenName),  //
                new Claim(ClaimTypes.Surname, user.Surname),      //
                new Claim(ClaimTypes.Role, user.Role)             //
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],// an open standard to pass data between client and server, and enables you to transmit data back and forth between the server and the consumers in a secure manner.
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);//JwtSecurityTokenHandler Converts a string into an instance of JwtSecurityToken. 
        }

        private UserModel Authenticate(UserLogin userLogin)
        {
            var currentUser = UserConstants.Users.FirstOrDefault(o => o.Username.ToLower() == userLogin.UserName.ToLower() && o.Password == userLogin.Password);

            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }
    }
}
