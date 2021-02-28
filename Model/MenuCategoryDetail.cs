using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arfler.Models
{
    public partial class MenuCategoryDetail
    {
        [Key]
        public long menuCategoryId { get; set; }

        [Display(Name = "Category Name")]
        public string menuCategoryName { get; set; }

        [Display(Name = "Restaurant")]
        public long restaurantId { get; set; }

        public long sortOrder { get; set; }

        public bool isEnabled { get; set; }

        public DateTime createdDate { get; set; }

        public DateTime? modifiedDate { get; set; }

        [Display(Name = "Description")]
        public string menuCategoryDesc { get; set; }

        [Display(Name = "Parent Category")]
        public long? parentMenuCategoryId { get; set; }

        [NotMapped]
        public string ParentCategoryName { get; set; }

        [NotMapped]
        public string RestaurantName { get; set; }

        public IEnumerable<MenuItemDetail> MenuItems { get; set; }
    }
}
