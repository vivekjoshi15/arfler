using System;
using System.Collections.Generic;

namespace Arfler.Models
{
    public partial class UserDetail
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string UserFirstName { get; set; }
        public string UserMiddleName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserPhoto { get; set; }
        public string UserAddress1 { get; set; }
        public string UserAddress2 { get; set; }
        public string UserCity { get; set; }
        public string UserState { get; set; }
        public string UserZipCode { get; set; }
        public string UserCountry { get; set; }
        public bool UserStatus { get; set; }
        public string UserPassword { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    
    }
}
