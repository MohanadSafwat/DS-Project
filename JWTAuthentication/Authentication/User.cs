using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthentication.Authentication
{
    public class User: IdentityUser
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [PersonalData]
        [Column(TypeName = "int")]
        public int Uid { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Address { get; set; }

        [PersonalData]
        [Column(TypeName = "int")]
        public int Amount { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Card { get; set; }

    }
}
