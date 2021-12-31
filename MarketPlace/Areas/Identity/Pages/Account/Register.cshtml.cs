using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using MarketPlace.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Web.Http;
using JWTAuthentication.Authentication;

namespace MarketPlace.Areas.Identity.Pages.Account
{

    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<User2> _userManager2;
        private readonly SignInManager<User2> _signInManager2;


        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            UserManager<User2> userManager2, 
            SignInManager<User2> signInManager2
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userManager2 = userManager2;
            _signInManager2 = signInManager2;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Card")]
            public string Card { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        
        public async Task OnGetAsync(string returnUrl = null)
        {
            
            
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

       
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                
                var client = new HttpClient();
                //string json = "{\"Email\":\"a99.b5dr123@gmail.com\",\"Username\":\"ahmed55edad\",\"Password\":\"Ahmed_123\"}";
                var json = JsonConvert.SerializeObject(Input);
                string uri = "http://localhost:61955/api/authenticate/register/";
                HttpResponseMessage request = await client.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
                var user = new User { UserName = Input.Email, Email = Input.Email , FirstName=Input.FirstName, LastName = Input.LastName, Address = Input.Address, Card = Input.Card };
                var user2 = new User2 { UserName = Input.Email, Email = Input.Email , FirstName=Input.FirstName, LastName = Input.LastName, Address = Input.Address, Card = Input.Card };
                //var result = await _userManager.CreateAsync(user, Input.Password);
                if (request.IsSuccessStatusCode)
                {
                    _logger.LogInformation("User created a new account with password.");
                    /*_userManager.RegisterTokenProvider("MyTokenProvider", new Token1<User>());
                    _userManager2.RegisterTokenProvider("MyTokenProvider", new Token2<User2>());
                    
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var code2 = await _userManager2.GenerateEmailConfirmationTokenAsync(user2);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    code2 = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code2));*/
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id/*, code = code*/, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                var ErrMsg = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.Content.ReadAsStringAsync().Result);

                //for (int i = 0;i< ErrMsg.Count; i++)
                //{
                    ModelState.AddModelError(string.Empty, ErrMsg["message"].ToString());
                // }

                
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
