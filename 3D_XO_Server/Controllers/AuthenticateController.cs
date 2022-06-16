using _3D_XO_Server.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace _3D_XO_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        //api/authenticate/login
        //api/authenticate/register
        //api/authenticate/register-admin

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("get-list-gamers-violation")]
        [Authorize(Roles = Authentication.UserRoles.Admin)]
        public IEnumerable<ApplicationUser> GetListWithViolations()
        {
            List<ApplicationUser> list = new List<ApplicationUser>();

            foreach (ApplicationUser u in userManager.Users)
            {
                if (u.Violation >= 0)
                    list.Add(u);
            }
            return list;
        }

        [HttpGet("get-user-by-username/{username}")]
        [Authorize]
        public async Task<ApplicationUser> GetUser(string username)
        {
            ApplicationUser user = await userManager.FindByNameAsync(username);
            return user;
        }

        [HttpPut("set-violation/{username}")]
        [Authorize]
        public async void SetViolation(string username)
        {
            ApplicationUser user = await userManager.FindByNameAsync(username);
            user.Violation++;
            if (user.Violation >= 10)
                user.IsActive = false;
            await userManager.UpdateAsync(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                if (user.IsActive == true)
                {
                    var userRoles = await userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                else
                    return NoContent();
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("login-in-game")]
        public async Task<IActionResult> LoginInGame(LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                if (user.IsActive == true)
                {
                    var userRoles = await userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        idUser = user.Id

                    });
                }
                else
                    return NoContent();
            }
            return Unauthorized();
        }

        [HttpPut("set-user/{username}&{isactive}")]
        [Authorize(Roles = Authentication.UserRoles.Admin)]
        public async Task<IActionResult> Put(string username, bool isactive)
        {
            ApplicationUser user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                user.IsActive = isactive;
                await userManager.UpdateAsync(user);
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost]
        [Route("register")]
        [Produces("application/xml")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return Ok(new ResponseModel { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                IsActive = true
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return Ok(new ResponseModel { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new ResponseModel { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        [Produces("application/xml")]
        [Authorize(Roles = Authentication.UserRoles.Admin)]
        public async Task<IActionResult> RegisterAdmin(RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                IsActive = true
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new ResponseModel { Status = "Success", Message = "User created successfully!" });
        }








    }
}
