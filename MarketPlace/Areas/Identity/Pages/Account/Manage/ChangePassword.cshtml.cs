using JWTAuthentication.Authentication;
using MarketPlace.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
namespace MarketPlace.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly UserManager<User2> _userManager2;
        private readonly SignInManager<User2> _signInManager2;
        public ChangePasswordModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager, 
            UserManager<User2> userManager2,
            SignInManager<User2> signInManager2,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _userManager2 = userManager2;
            _signInManager2 = signInManager2;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            
            dynamic user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                user = await _userManager2.GetUserAsync(User);
                if (user == null)
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            dynamic hasPassword;
            try { hasPassword = await _userManager.HasPasswordAsync(user);
            }
            catch(Exception e)
            {
                hasPassword = await _userManager2.HasPasswordAsync(user);
            }
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            dynamic user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                user = await _userManager2.GetUserAsync(User);
                if (user == null)
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            dynamic changePasswordResult;
            try
            {
                changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            }
            catch (Exception e)
            {
                changePasswordResult = await _userManager2.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            }
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            //await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            return RedirectToPage();
        }
    }
}
