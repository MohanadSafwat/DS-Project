using JWTAuthentication.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<User> _signInManager;
        public AuthenticateController(SignInManager<User> signInManager, ApplicationDbContext db,UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _db = db;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                   // new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("firstName", user.FirstName),
                    new Claim("lastName", user.LastName),
                    new Claim("amount", user.Amount.ToString()),
                    new Claim("card", user.Card),
                    new Claim(ClaimTypes.StreetAddress, user.Address),
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
                    expires: DateTime.Now.AddMonths(5),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    Status = 200
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            User user = new User()
            {
                UserName = model.Email,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                Card = model.Card,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            User user = new User()
            {
                UserName = model.Email,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                Card = model.Card,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }


        [HttpGet]
        [Route("getUser/{email}")]

        public async Task<IActionResult> GetUser(string email)
        {
            var userExists = await userManager.FindByEmailAsync(email);
            if (userExists == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email Not exists!" });   
            
            return Ok(userExists);
        }


        [HttpGet]
        [Route("getUser")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var currentUser = GetCurrentUser();
              

            return Ok(currentUser);
        }

        private User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                /*return Ok(new
                {
                    token = new User
                    {
                        FirstName = userClaims.FirstOrDefault(o => o.Type == "firstName")?.Value,
                        LastName = userClaims.FirstOrDefault(o => o.Type == "lastName")?.Value,
                        Amount = Convert.ToInt32(userClaims.FirstOrDefault(o => o.Type == "amount")?.Value),
                        Card = userClaims.FirstOrDefault(o => o.Type == "card")?.Value,
                        Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                        Address = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.StreetAddress)?.Value,
                        *//*Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                        Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value*//*
                    },
                "status:"200"
                });*/
                return new User
                {
                    FirstName = userClaims.FirstOrDefault(o => o.Type == "firstName")?.Value,
                    LastName = userClaims.FirstOrDefault(o => o.Type == "lastName")?.Value,
                    Amount = Convert.ToInt32(userClaims.FirstOrDefault(o => o.Type == "amount")?.Value),
                    Card = userClaims.FirstOrDefault(o => o.Type == "card")?.Value,
                    Email  = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Address = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.StreetAddress)?.Value,
                    /*Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value*/
                };
            }
            return null;
        }
        [HttpGet]
        [Route("getCode/{email}")]
        public async Task<IActionResult> GetCode(string email)
        {
            var userExists = await userManager.FindByEmailAsync(email);
            if (userExists == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email Not exists!" });
            var code = await userManager.GenerateEmailConfirmationTokenAsync(userExists);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            return Ok(code);
        }

        [HttpGet]
        [Route("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string code)
        {
            var userExists = await userManager.FindByEmailAsync(email);
            if (userExists == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email Not exists!" });
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await userManager.ConfirmEmailAsync(userExists, code);
            if(result.Succeeded)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Invalid Token" });

        }

        void setFirstName(User user, String name)
        {
            user.FirstName = name;
        }
        void setLastName(User user, String name)
        {
            user.LastName = name;
        }
        void setAddress(User user, String addr)
        {
            user.Address = addr;
        }

        [HttpPut]
        [Route("updateProfile/{email}")]
        public async Task<IActionResult> UpdateProfile(string email, [FromBody] UpdateModel model)
        {
            var userExists = await userManager.FindByEmailAsync(email);
            if (userExists == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Not exists!" });

            var phoneNumber = await userManager.GetPhoneNumberAsync(userExists);
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await userManager.SetPhoneNumberAsync(userExists, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Phone Error" });
                }
            }

            if (model.FirstName != userExists.FirstName && model.FirstName.Length != 0)
            {
                setFirstName(userExists, model.FirstName);
                _db.SaveChanges();
            }
            if (model.LastName != userExists.LastName && model.LastName.Length != 0)
            {
                setLastName(userExists, model.LastName);
                _db.SaveChanges();
            }
            if (model.Address != userExists.Address && model.Address.Length != 0)
            {
                setAddress(userExists, model.Address);
                _db.SaveChanges();
            }

            await _signInManager.RefreshSignInAsync(userExists);

            return Ok(new Response { Status = "Success", Message = "User updated successfully!" });
        }

        [HttpPut]
        [Route("resetPassword/{email}")]
        public async Task<IActionResult> ResetPassword(string email, [FromBody] ChangePasswordModel model)
        {
            var userExists = await userManager.FindByEmailAsync(email);
            if (userExists == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Not exists!" });

            var changePasswordResult = await userManager.ChangePasswordAsync(userExists, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Error in changing password" });

            }

            await _signInManager.RefreshSignInAsync(userExists);


            return Ok(new Response { Status = "Success", Message = "Password updated successfully!" });
        }

        [HttpPut]
        [Route("addMoney/{email}")]
        public async Task<IActionResult> AddMoney(string email, [FromQuery] int money)
        {
            var userExists = await userManager.FindByEmailAsync(email);
            if (userExists == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Not exists!" });

            userExists.Amount += money;
            _db.SaveChanges();

            await _signInManager.RefreshSignInAsync(userExists);

            return Ok(new Response { Status = "Success", Message = "Money added successfully!" });
        }



    }
}
