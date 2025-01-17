using DevOpsExmaProject.Identity.Dtos;
using DevOpsExmaProject.Identity.Entitys;
using DevOpsExmaProject.Identity.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevOpsExmaProject.Identity.Controllers
{
    [Route("Register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly ISMTPService _sMTPService;
        private readonly IConfiguration _configuration;

        public RegisterController(UserManager<User> userManager, SignInManager<User> signInManager, IUserService userService, ISMTPService sMTPService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _sMTPService = sMTPService;
            _configuration = configuration;
        }

     

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInDto dto)
        {
            User user = await _userService.GetAsync(u => u.Email == dto.Email && u.UserName == dto.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var token = GetToken(authClaims);

                var testToken = new { Token = new JwtSecurityTokenHandler().WriteToken(token), Expiration = token.ValidTo, UserId = user.Id };

                 return Ok(testToken);

            }
            else
            {
                return Ok("not found");
            }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpDto dto)
        {
            User user = new()
            {
                UserName = dto.UserName,
                Email = dto.Email,
            };

            var result = await _userManager.CreateAsync(user, dto.Password!);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(result.Succeeded);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("checkEmail")]
        public async Task<IActionResult> CheckEmail(string Email)
        {
            if (await _userService.GetAsync(u => u.Email == Email) == null)
            {
                return Ok("false");
            }
            return Ok("true");
        }

        [HttpPost("checkSMTP")]
        public async Task<IActionResult> CheckSMTP(CheckSMTPDto dto)
        {

            int randomNumber = Random.Shared.Next(100000, 999999);
            _sMTPService.SendSMTP(dto.Email!, randomNumber);

            int encryptionRandomNumber = randomNumber + 121212;

            return Ok(encryptionRandomNumber);
            
        }



        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT Key is missing in the configuration.");
            }

            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                expires: DateTime.UtcNow.AddHours(12),  
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

    }
}
