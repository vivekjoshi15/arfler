using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arfler.Models
{
    public class RestaurantDetail
    {
        [Key]
        public long id { get; set; }
        public string restaurantName { get; set; }
        public string categoryIds { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipCode { get; set; }
        public DateTime createdDate { get; set; }
        public string mainImageUrl { get; set; }
        public bool isEnable { get; set; }
        public DateTime modifiedDate { get; set; }
        public string Description { get; set; }
        public string intro { get; set; }
        public long sortOrder { get; set; }
        public string userId { get; set; }
        public string tagline { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string timing1 { get; set; }
        public string timing2 { get; set; }
        public string timing3 { get; set; }
    }
}
