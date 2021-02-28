using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Arfler.Models
{
    public class PeopleDetail
    {
        public long id { get; set; }
        public long restaurantId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public bool isEnable { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime modifiedDate { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string designation { get; set; }
        public string imageUrl { get; set; }
        public long sortOrder { get; set; }
        public string userId { get; set; }
        public string facebookUrl { get; set; }
        public string twitterUrl { get; set; }
    }
}
