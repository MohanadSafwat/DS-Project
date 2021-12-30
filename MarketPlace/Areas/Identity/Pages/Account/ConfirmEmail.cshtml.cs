using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MarketPlace.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
using Newtonsoft.Json;

namespace MarketPlace.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public ConfirmEmailModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string userId, string code, string userId2, string code2)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }
            var client = new HttpClient();
            //string json = "{\"Email\":\"a99.b5dr123@gmail.com\",\"Username\":\"ahmed55edad\",\"Password\":\"Ahmed_123\"}";
            //var json = JsonConvert.SerializeObject(Input);
            string uri = "http://localhost:61955/api/authenticate/getUser/" + email;
            string confrimUri = "http://localhost:61955/api/authenticate/confirmEmail?email=" + email + "&code=" + code;

            HttpResponseMessage response = await client.GetAsync(uri);
            
            
            var user = response.Content.ReadAsStringAsync().Result;
            var user2 = await _userManager.FindByIdAsync(userId2);
            if (user == null || user2 ==  null)
            {
                return NotFound($"Unable to load user with Email '{email}'.");
            }

            code2 = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code2));
            //var result = await _userManager.ConfirmEmailAsync(user2, code2);
            HttpResponseMessage confirmResponse = await client.GetAsync(confrimUri);
            StatusMessage = confirmResponse.IsSuccessStatusCode ? "Thank you for confirming your email." : "Error confirming your email.";
            return Page();
        }
    }
}
