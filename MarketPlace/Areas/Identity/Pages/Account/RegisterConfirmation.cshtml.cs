using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using JWTAuthentication.Authentication;

namespace MarketPlace.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _sender;
        private readonly UserManager<User2> _userManager2;
        private readonly SignInManager<User2> _signInManager2;
        
        


        public RegisterConfirmationModel(UserManager<User> userManager, IEmailSender sender, UserManager<User2> userManager2,
        SignInManager<User2> signInManager2)
        {
            _userManager = userManager;
            _sender = sender;
            _userManager2 = userManager2;
            _signInManager2 = signInManager2;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var client = new HttpClient();
            string uri = "http://localhost:61955/api/authenticate/getUser/" + email;
            string codeUri = "http://localhost:61955/api/authenticate/getCode/" + email;
            HttpResponseMessage response = await client.GetAsync(uri);
            HttpResponseMessage codeResponse = await client.GetAsync(codeUri);
            var user = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);
            var codeJson = codeResponse.Content.ReadAsStringAsync().Result;
            //var user = request.Content;
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            dynamic user2 = await _userManager.FindByEmailAsync(email);
                if(user2 == null)
                    user2 = await _userManager2.FindByEmailAsync(email);

            Email = email;
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            DisplayConfirmAccountLink = true;
            if (DisplayConfirmAccountLink)
            {
                var userId = user["id"];
                //var userId2 = user2.Id;
                /*_userManager.RegisterTokenProvider("MyTokenProvider", new Token1<User>());
                _userManager2.RegisterTokenProvider("MyTokenProvider", new Token2<User2>());
                if(user2.Address == "North" || user2.Address == "north")
                    dynamic code2 = await _userManager.GenerateEmailConfirmationTokenAsync(user2);
                code2 = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code2));*/
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity",email = email ,userId = userId, code = codeJson,/*code2 =code2, userId2 = userId2,*/ returnUrl = returnUrl },
                    protocol: Request.Scheme);
            }

            return Page();
        }
    }
}
