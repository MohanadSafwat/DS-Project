using JWTAuthentication.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly UserManager<User2> userManager2;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly ApplicationDb2Context _db2;
        private readonly SignInManager<User> _signInManager;
        public AuthenticateController(SignInManager<User> signInManager, ApplicationDbContext db, ApplicationDb2Context db2, UserManager<User> userManager, UserManager<User2> userManager2, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.userManager2 = userManager2;
            this.roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _db = db;
            _db2 = db2;
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
                    expires: DateTime.Now.AddMonths(5),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {



            if (model.Address == "North" || model.Address == "north")
            {
                var userExists = await userManager.FindByNameAsync(model.Email);
                var userExists2 = await userManager2.FindByNameAsync(model.Email);
                if (userExists != null || userExists2 != null)
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
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please use at least one uppercase and lowercase and a symbol in the password" });

            }
            else if (model.Address == "South" || model.Address == "south")
            {
                var userExists = await userManager.FindByNameAsync(model.Email);
                var userExists2 = await userManager2.FindByNameAsync(model.Email);
                if (userExists != null || userExists2 != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
                User2 user = new User2()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    Card = model.Card,
                };
                var result = await userManager2.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please use at least one uppercase and lowercase and a symbol in the password" });

            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Address must be North or South" });

            }






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
            var userExists2 = await userManager2.FindByEmailAsync(email);
            if (userExists == null && userExists2 == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email Not exists!" });

            
            if (userExists != null) return Ok(userExists);
            else return Ok(userExists2);

        }

        [HttpGet]
        [Route("getCode/{email}")]
        public async Task<IActionResult> GetCode(string email)
        {
            var userExists = await userManager.FindByEmailAsync(email);
            var userExists2 = await userManager2.FindByEmailAsync(email);
            if (userExists == null && userExists2 == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email Not exists!" });
            if (userExists != null)
            {
                var code = await userManager.GenerateEmailConfirmationTokenAsync(userExists);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                return Ok(code);
            } else
            {
                var code = await userManager2.GenerateEmailConfirmationTokenAsync(userExists2);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                return Ok(code);
            }
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
            if (result.Succeeded)
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
