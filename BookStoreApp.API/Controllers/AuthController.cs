using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> usermanager;
        private readonly IConfiguration configuration;

        public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<ApiUser> usermanager, IConfiguration configuration)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.usermanager = usermanager;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        //public async Task<IActionResult> Login(string returnUrl)
        public async Task<IActionResult>Register(UserDto userDto)
         {

            logger.LogInformation($"Registration Attempt for {userDto.Email}");
            try
            {

                var user = mapper.Map<ApiUser>(userDto);
                user.UserName = userDto.Email;
                var result = await usermanager.CreateAsync(user, userDto.Password);
                if (result.Succeeded == false)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }


                //if(string.IsNullOrEmpty(userDto.Role))
                // {

                await usermanager.AddToRoleAsync(user, "User");

                // }
                //else
                // {

                //     await usermanager.AddToRoleAsync(user, "User");
                // }
                return Accepted();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Somthing went wrong in the {nameof(Register)}");
                return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);

            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>>Login(LoginUserDto userDto)
        {
            logger.LogInformation($"Registration Attempt for {userDto.Email}");
            try
            {
                var user = await usermanager.FindByEmailAsync(userDto.Email);
                var passwordValid = await usermanager.CheckPasswordAsync(user, userDto.Password);
                if (user == null || passwordValid == false)
                {
                    return Unauthorized(userDto);  
                }

                string token = await GenerateToken(user);


                var response = new AuthResponse
                {
                    Email = userDto.Email,
                    Token = token,
                    UserId = user.Id

                };
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Somthing went wrong in the {nameof(Register)}");
                return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);

            }
        }

        private async Task<string> GenerateToken(ApiUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await usermanager.GetRolesAsync(user);

            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role,q)).ToList();
            var userClaims = await usermanager.GetClaimsAsync(user);
            var claims = new List<Claim>
            {
                 new Claim( JwtRegisteredClaimNames.Sub ,user.UserName),
                 new Claim( JwtRegisteredClaimNames.Jti ,Guid.NewGuid().ToString()),
                 new Claim( JwtRegisteredClaimNames.Email ,user.Email),
                 new Claim( CustomClaimTypes.Uid ,user.Id)


            }.Union(userClaims)
             .Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                //expires: DateTime.UtcNow.AddHours(Convert.ToInt32(configuration["JwtSetting:Duration"])),
                expires: DateTime.UtcNow.AddHours(5),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
    }
    }
    

}
