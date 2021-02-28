using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arfler.Models
{
    public class ImageDetail
    {
        public long id { get; set; }
        public long restaurantId { get; set; }
        public string imageUrl { get; set; }
        public bool isEnable { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime modifiedDate { get; set; }
        public string userId { get; set; }
        public long sortOrder { get; set; }
        [NotMapped]
        public string RestaurantName { get; set; }
    }
}
