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

namespace MarketPlace.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _sender;

        public RegisterConfirmationModel(UserManager<User> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
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

            User user2 = await _userManager.FindByEmailAsync(email);

            Email = email;
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            DisplayConfirmAccountLink = true;
            if (DisplayConfirmAccountLink)
            {
                var userId = user["id"];
                var userId2 = user2.Id;
                var code2 = await _userManager.GenerateEmailConfirmationTokenAsync(user2);
                code2 = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code2));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity",email = email ,userId = userId, code = codeJson,code2 =code2, userId2 = userId2, returnUrl = returnUrl },
                    protocol: Request.Scheme);
            }

            return Page();
        }
    }
}
