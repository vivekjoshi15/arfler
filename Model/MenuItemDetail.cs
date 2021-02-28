using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arfler.Models
{
    public class MenuItemDetail
    {
        [Key]
        public long menuItemId { get; set; }
        public string menuItemName { get; set; }
        public string menuItemDesc { get; set; }
        public long menuCategoryId { get; set; }
        public string menuItemRate { get; set; }
        public long sortOrder { get; set; }
        public DateTime createdDate { get; set; }
        public string menuItemImageUrl { get; set; }
        public long restaurantId { get; set; }

        [NotMapped]
        public string MenuItemCategoryName { get; set; }

        [NotMapped]
        public string restaurantName { get; set; }

        //public bool isEnabled { get; set; }
    }
}
