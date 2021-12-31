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
        private readonly UserManager<User2> _userManager2;
        private readonly SignInManager<User2> _signInManager2;
        private readonly AppDBContext _db;
        private readonly AppDB2Context _db2;
        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            AppDBContext db,
            AppDB2Context db2,
            UserManager<User2> userManager2,
            SignInManager<User2> signInManager2



            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userManager2 = userManager2;
            _signInManager2 = signInManager2;
            _db = db;
            _db2 = db2;
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
            /*[Display(Name = "Address")]
            public string Address { get; set; }*/
        }

       void setFirstName(User user , String name)
        {
            user.FirstName = name;
        }
        void setLastName(User user, String name)
        {
            user.LastName = name;
        }
        /*void setAddress(User user, String addr)
        {
            user.Address = addr;
        }*/

        void setFirstName(User2 user, String name)
        {
            user.FirstName = name;
        }
        void setLastName(User2 user, String name)
        {
            user.LastName = name;
        }
        /*void setAddress(User2 user, String addr)
        {
            user.Address = addr;
        }*/
        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var first =  user.FirstName;
            var last =  user.LastName;
            

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName=first,
                LastName=last,
                

            };
        }
        private async Task LoadAsync(User2 user)
        {
            var userName = await _userManager2.GetUserNameAsync(user);
            var phoneNumber = await _userManager2.GetPhoneNumberAsync(user);
            var first = user.FirstName;
            var last = user.LastName;
            

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = first,
                LastName = last,


            };
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

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            dynamic user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                user = await _userManager2.GetUserAsync(User);
                if(user == null)
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            dynamic phoneNumber ;
            try
            {
                phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            }
            catch (Exception e)
            {
                phoneNumber = await _userManager2.GetPhoneNumberAsync(user);
            }
            if (Input.PhoneNumber != phoneNumber)
            {
                dynamic setPhoneResult;
                try
                {
                    setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                }
                catch (Exception e)
                {
                    setPhoneResult = await _userManager2.SetPhoneNumberAsync(user, Input.PhoneNumber);
                }
                //var setPhoneResult = await userManager.SetPhoneNumberAsync(userExists, model.PhoneNumber);

                if (!setPhoneResult.Succeeded)
                {

                     StatusMessage = "Phone error";
                }
            }

            if (Input.FirstName != user.FirstName && Input.FirstName.Length !=0) {
                setFirstName(user, Input.FirstName);
                _db.SaveChanges(); _db2.SaveChanges();
            }
            if (Input.LastName != user.LastName && Input.LastName.Length != 0)
            {
                setLastName(user, Input.LastName);
                _db.SaveChanges(); _db2.SaveChanges();
            }
            

            //await _signInManager.RefreshSignInAsync(user);
            //await _signInManager2.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
