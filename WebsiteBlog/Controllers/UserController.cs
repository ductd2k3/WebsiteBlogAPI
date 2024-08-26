using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using WebsiteBlog.Data;
using WebsiteBlog.DTO;
using WebsiteBlog.Models;
using WebsiteBlog.Repository;

namespace WebsiteBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserReposiroty _userRepository;
        private readonly string secretKey;
        public UserController(IUserReposiroty userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            secretKey = configuration["JWTSettings:SecretKey"];
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] DTOUser user)
        {
            if(user.Email == null || user.Password == null || user.Email.Length == 0 || user.Password.Length == 0) 
            {
                return Ok(new Response { Success = "false" });
            }
            User _user =  _userRepository.GetUserByEmailPassword(user.Email, user.Password);
            if (_user == null)
            {
                return Ok(new Response { Success = "false"});
            }
            return Ok(new Response { Success = "true", Data = GenarateToken(_user)});
        }
        [HttpPost("Register")]
        public IActionResult Register([FromBody] DTOUser DTOUser) 
        {
            string username = DTOUser.UserName;
            string email = DTOUser.Email;
            string password = DTOUser.Password;
            string regexEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            var _regexEmail = new Regex(regexEmail);
            string regexPassword = @"(?=(?:.*\d){6})";
            var _regexPassword = new Regex(regexPassword);
            if(!_regexEmail.IsMatch(email))
            {
                return Ok(new Response { Success = "false", Data = "Error email format!"});
            }
            if (!_regexPassword.IsMatch(password))
            {
                return Ok(new Response { Success = "false", Data = "At least 6 character for password!" });
            }
            bool duplicate = _userRepository.GetUserByEmail(email) != null;
            if (duplicate)
            {
                return Ok(new Response { Success = "false", Data = "Email already exist!" });
            }
            _userRepository.AddUser(username, email, password);
            return Ok(new Response { Success = "true" });
        }
        [HttpPut("Update")]
        [Authorize]
        public IActionResult UpdateUser([FromBody] DTOUser DTOUser)
        {
            string username = DTOUser.UserName;
            string email = DTOUser.Email;
            string password = DTOUser.Password;
            string image = DTOUser.Image;
            User user = _userRepository.GetUserByEmail(email);
            user.UserName = username ?? user.UserName;
            user.Password = password ?? user.Password;
            user.UserImage = image ?? user.UserImage;
            
            string regexPassword = @"(?=(?:.*\d){6})";
            var _regexPassword = new Regex(regexPassword);
            
            if (!_regexPassword.IsMatch(user.Password))
            {
                return Ok(new Response { Success = "false", Data = "At least 6 character for password!" });
            }

            _userRepository.UpdateUser(user);
            User _user = _userRepository.GetUserByEmail(email);
            return Ok(new Response { Success = "true", Data = _user});
        }

        private string GenarateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyByte = Encoding.UTF8.GetBytes(secretKey);
            string role = user.role == 1 ? "admin" : "customer";
            var tokenDescriptrion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", user.UserId.ToString()),
                    new Claim("userName", user.UserName),
                    new Claim("email", user.Email),
                    new Claim("role", role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyByte),SecurityAlgorithms.HmacSha256)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptrion);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
