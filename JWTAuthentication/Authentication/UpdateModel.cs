using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Authentication
{
    public class UpdateModel
    {
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }


        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        

    }
}
