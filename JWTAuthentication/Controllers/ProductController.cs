using JWTAuthentication.Authentication;
using MarketPlace.Models;
using MarketPlace.Models.Repositories;
using MarketPlace.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<User> _signInManager;
        private readonly IProductRepository<Product> productRepository;
        [Obsolete]
        private readonly IHostingEnvironment hosting;
        private readonly IAssociatedRepository<AssociatedSell> associatedSellRepository;
        private readonly IAssociatedRepository<AssociatedShared> associatedSharedRepository;
        private readonly IAssociatedRepository<AssociatedBought> associatedBoughtRepository;
        private ApplicationDbContext db;
        [Obsolete]
        public ProductController(IAssociatedRepository<AssociatedSell> associatedSellRepository,
            IAssociatedRepository<AssociatedShared> associatedSharedRepository,
            IAssociatedRepository<AssociatedBought> associatedBoughtRepositor,
            IProductRepository<Product> productRepository,
            IHostingEnvironment hosting,
            SignInManager<User> signInManager, ApplicationDbContext db,UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _db = db;
            this.productRepository = productRepository;
            this.hosting = hosting;
            this.associatedSellRepository = associatedSellRepository;
            this.associatedSharedRepository = associatedSharedRepository;
            this.associatedBoughtRepository = associatedBoughtRepositor;//this updated
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public  ActionResult GetAllProducts()
        {
            
            var productsIndex = associatedSellRepository;


            var model = new 
            {
                productsIndex = associatedSellRepository.List(),

            };

            return Ok(model);
        }
        public async Task<User> UserReturn(string id)
        {
            return await userManager.FindByIdAsync(id);

        }

        [HttpPost]
        [Route("createProduct")]
        public async Task<ActionResult> createProduct([FromForm] ProductViewModel model)
        {
            try
            {
                string fileNames = string.Empty;
                if (model.Files != null)
                {
                    /*                    var file = model.Files;
                    */
                    foreach (var file in model.Files)
                    {
                        string upload = Path.Combine(hosting.WebRootPath, "images");
                        fileNames += (file.FileName);
                        fileNames += "`";
                        string fullPath = Path.Combine(upload, file.FileName);
                        file.CopyTo(new FileStream(fullPath, FileMode.Create));
                    }
                }

                Product product = new Product
                {
                    ProductBrand = model.ProductBrand,
                    ProductDescription = model.ProductDescription,
                    ProductName = model.ProductName,
                    ProductPrice = model.ProductPrice,
                    ProductImageUrls = fileNames,
                };

                productRepository.Add(product);

                int id = product.ProductId;

                Task<User> Seller = UserReturn(model.SellerId);
                User userData = await Seller;

                AssociatedSell associatedSell = new AssociatedSell
                {
                    productId = product,
                    SellerId = userData,
                    Sold = false
                };

                associatedSellRepository.Add(associatedSell);

                return Ok(product);


            }
            catch { return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error"}); }
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
