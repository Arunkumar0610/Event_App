using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserAuthenticatonService.Models;
namespace UserAuthenticatonService.Repositories
{
    public class AuthRepository:IAuthRepository
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration _configuration;
        public AuthRepository(UserManager<IdentityUser> _userManager
            ,IConfiguration configuration) 
        {
            userManager = _userManager;
            _configuration = configuration;
        }
        public async Task<LoginResponse> Login(Login login)
        {
            var user = await userManager.FindByNameAsync(login.UserName);
            if(user!=null && await userManager.CheckPasswordAsync(user,login.Password))
            {

                var token = GetToken(user);
                LoginResponse response = new ()
                {
                    Token = token,
                    UserName = user.UserName
                };
                return response;
            }
            else
            {
                return new LoginResponse();
            }
        }
        public async Task<Response> RegisterUser(User user)
        {
            var userExists = await userManager.FindByNameAsync(user.UserName);
            if (userExists != null)
                return  new Response { Status = "Error", Message = "User already exists!" };
            IdentityUser identityuser = new()
            {
                Email = user.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = user.UserName
            };
            var result = await userManager.CreateAsync(identityuser, user.Password);
            if (!result.Succeeded)
                return new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." };
            return new Response { Status = "Success", Message = "User created successfully!" };
        }
        private string GetToken(IdentityUser user)
        {
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "User"),
                new Claim("UserName",user.UserName.ToString())
            };
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["Jwt:ValidAudience"],
                expires:DateTime.Now.AddMinutes(60),
                claims:claims,
                signingCredentials:creds
                );
            return  new JwtSecurityTokenHandler().WriteToken(token); ;
        }
    }
}
