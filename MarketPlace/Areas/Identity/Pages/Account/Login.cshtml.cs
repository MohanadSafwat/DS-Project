using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MarketPlace.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using RestSharp;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using JWTAuthentication.Authentication;

namespace MarketPlace.Areas.Identity.Pages.Account
{
    [Authorize]
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly UserManager<User2> _userManager2;
        private readonly SignInManager<User> _signInManager;
        private readonly SignInManager<User2> _signInManager2;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<User> signInManager, SignInManager<User2> signInManager2,
            ILogger<LoginModel> logger,
            UserManager<User> userManager, UserManager<User2> userManager2)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userManager2 = userManager2;
            _signInManager2 = signInManager2;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        
        
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }
       
        
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {

                var client = new HttpClient();
                
                var json = JsonConvert.SerializeObject(Input);
                string uri = "http://localhost:61955/api/authenticate/login/";
                //HttpResponseMessage request = await client.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                var result2 = await _signInManager2.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                
                if (/*request.IsSuccessStatusCode &&*/ result.Succeeded || result2.Succeeded)
                {
                    //var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.Content.ReadAsStringAsync().Result);
                    /*await HttpContext.SignInAsync(data["token"].ClaimsPrincipal,
           data["token"].AuthProperties);*/
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", data["token"]);
                   //HttpResponseMessage res = await client.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));

                    //var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString();/*.Replace("Bearer ", "");*/

                    /*HttpContext.Response.Cookies.Append(".AspNetCore.Identity.Application", "CfDJ8Mvg7J5jp2RPvypz55Zu-YM1BTmQbu4FqOctpS3WhWv6hRV91hvYlYs5DObeNHiLNCBiwQpt1b25bpWQWBFzvQkNIFpAyc6M8qq17nbqqSh4rySOqbhfTHp7kdhZuHYRqK8j6aESN6gYA9CT0U_Jz9uic4GtU5RxxmQnC7IAP3NaYm_CRF7C3wn0nY_RWTffeedlz2WZxu5glXKRGOLyyNN478Y_rsBnHgmBcqoTYWBhANSnLR0FQJ5C7lYScccMi-NA2OWt8b9407E3Iu3X5d9fUiPDWgbuHh2MVgI6n-kir2xRED6fo3JhYN_wt5p5Y8dmX2idktwamlkPU6N9cj7pyrFbM4MniRsWYEu2fLpPeANuPVk9EbS12ZXuPmxC-pAIlTySCHumde4iYe79B1reVX2GGhpG54-a2nHKMbXsCkvd5JGHuwjs8e3Krz6wxtDjnsCthktUXgME610nsbtVN8NVhjET1M-BFkqHn6Y-lFoNXhlZI9Y0qdGDq_eEBC1mFdaC8LU9L33UjzvG27hIDUoApJe3BeeQRRVTRANGyB5vFN5nDrxNt0dOUzbCw86Mn85XpQyfM6PclLmjApk",
                    new Microsoft.AspNetCore.Http.CookieOptions()
                    {
                        Path = "/"
                    });*/
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                //
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
        //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYTk5LmJkMjJyOTk3OUBnbWFpbC5jb20iLCJqdGkiOiJhNDcxNmI0ZC01N2ExLTRmMDItOWE4Yy1mMjhjMzEwMzUzZjIiLCJleHAiOjE2NDA3MTc5MjMsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NjE5NTUiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjQyMDAifQ.lHCxuCVh2TKikD5DiV1qVCsgHNlClUe9JTXL2PsXNGg
        //CfDJ8Mvg7J5jp2RPvypz55Zu-YOnDIHoadkJKH8EFhl21gj-YenYcT5xySAuPYrCn9VBMNDVYg7FpDOtA-Wj223Kbr2AVBLR9OgjTeTH6P98AJ2t6zOMKE_Padyc69gxYS9_3BzwIqUqqo1DAVbhtoSZYFxFFvHCkAunhOiJTBgKeFwH0sIOfbt4qQZO7hbVBr3RgxKZ44r7jtfsUST8ctQyMi4PzmzemDV-_BAfAfP9IuqG5_ModG8FdBc4cF0DVDS8PZI6cCwWA2Lq-HdY2nBZuQJF7TBMFbDlCve_kiJkDc0Z86dbmb0j9Kq_tGWe85FBF1WmLBnao6mwRfs7T96gs_1h2s0lMYOD2lOqqGP04m8XeFRZFq1oBYVwEyPKh3KezOwZ5W6uFblCtXFGEFKKfCOgV0a7I1eu54-FM5-QDY7n0xoLzILu6wY5yieWnopop53kpnYMBLtoFdVU1L3nkte46MSEbwR41W6747jW1g3FXH13gf8_uNrlh-ROeXp-QMns-l3_zRLi1HqA8v90NQSycL8L2DhfDGwy54Q9dd2FFzwwI6rUTA_1M-AzTxKMwBbrAByAeh6L0qcTcsL78lxMnQ3WfjyKgqjw2Gz5cKLxvI3LS0B0EKlPcMv7QzxPnQ
        //CfDJ8Mvg7J5jp2RPvypz55Zu-YMm-FB00EIbW4zMUOZMQX8yTJB2-cBk0AgYHrEd2tBe8U1G5qWl4wlu96aIGxUhlcrjU9NNx4XcNmhivBKEzeZh1O2Bdqlytrnx-An6EnVZGoxIrM7yX2B8HMjB7N8-tpIZ68XPO8xVWXxR_L5a2YNcrBpnJIGQr5VsmtdQIhl901osFe3CKtxSa3JGhF8h7UdS2duqQNpc_P4VuWko7EB4ZDN3CbwRz8bzBG7APzaxDHpGbhueH9_cl4SZa-zX9LSKUAy3Ks2IrDiOBTm-xk5g3QYUy88EKjvoVvv9N2GO_qyDhDbv1GiWmd3XnPbcTEXp7GnBpulgFIExu46O0jLkQE2rvl75IpjWIL2v6BkGJWEJDTsrxM3yIO0JUcotSGYps3kRNAAaix80oukw9FtG49WDBt7bD5ZPAkEyA3AuB6UN3-Yom8kBoskMzYjZEsMRrqbmAN6IGkqmJwj6WFH1OmKEu4QEgJYhJVJ5v4eX4NYZMZc00m0jNxRLVZMxKWtwiuZ7rL6L-ZiCjAILpnLnba_s1E8MKkrC5tXwWlhSx6D0x0N9BHxxrxHqFlsrWxSxSTdycMh7bERMTnbnRS62obgz5BpR4QOpRlDJ7CnI-A
        //CfDJ8Mvg7J5jp2RPvypz55Zu-YOS9OqwId_m1dXxIMI9QrZiQe-aX4OJ81c1cfXkSnkVAFJQEGCx6qGiHNZOUN6t2MzQJTdKyW3Zz9mvgfrPd6sqAC9Zfxd3XgTGhQqtI4dgU4wuSK0biusWYArGTNv2J8w
        //CfDJ8Mvg7J5jp2RPvypz55Zu-YPZHOrfL02_ecz2cG9-FszrhHazOSXbfU_dwusoLvf7K0W854QGE_5mu-mPJfvrLSlazWfI6a1lHnRGQb5aTQ_jO7uLZ7r2bzvH4gb043YYHCR9ddAKufhk2ybsbZ_TKcCj4D_rLSbkF5DXqOB1RCLM75-Q8_9n2dQTJlSNMJMD8tXKNzKg8pnp_CCXybyD14ZVyeSqL_wSAJDKfOB-B-Lgp35ZYPZEllKAn7BJtzesGFaakKz0Rf5NAdG7g17ZI6A8NVCCsQcrUMh0PePAwAo-6er3xKXcDrngsLXv_6jDx3SJO3YyzDS7Ek2EES73vptfyP6ZTXwT7rW_dLIZCePkr_gBeNlec9BDMf-gDdhVILIqtGu9RVvt6yvYii5C-fyHUFhm95IRUtKBskcxknqe8uKaCGBfAr-J3k2-uNQNwKgb6MUUcTUcG9FgrxMKnUTbkrxQgsWCPDHdTqkCnlmvTUbQWkFeekcTRLEQcsmC3ZJ0NRecSX-B4E1Xs8wN845eA8DON4lfODli5-8l4apVikekTXoNj-Pei13PLUYQrwKN0xXXPkP-SBeU4lapvyU37VtZ75I10krCNjXNlJxcrqWHGdV22ovLLlIt397zKQ	
        /*[HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            //var branch = new Branch
            //{
            //    branchName = "Regie",
            //    address = "Naval"

            //};
            //branchContext.Branch.Add(branch);
            //branchContext.SaveChanges();

            return Page();
        }

        [Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(InputModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return Page();
        }*/
    }
}
