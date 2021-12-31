using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JWTAuthentication.Authentication;
using MarketPlace.Areas.Identity.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarketPlace.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDBContext _db;
        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            AppDBContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Display(Name = "Address")]
            public string Address { get; set; }
        }

       void setFirstName(User user , String name)
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
        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var first =  user.FirstName;
            var last =  user.LastName;
            var address =  user.Address;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName=first,
                LastName=last,
                Address=address

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.FirstName != user.FirstName && Input.FirstName.Length !=0) {
                setFirstName(user, Input.FirstName);
                _db.SaveChanges();
            }
            if (Input.LastName != user.LastName && Input.LastName.Length != 0)
            {
                setLastName(user, Input.LastName);
                _db.SaveChanges();
            }
            if (Input.Address != user.Address && Input.Address.Length != 0)
            {
                setAddress(user, Input.Address);
                _db.SaveChanges();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
