using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Arfler.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public enum userRoles
    {
        Admin,
        User,
        Merchant
    }

}
