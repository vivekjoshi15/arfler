using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arfler.Models
{
    public partial class CategoryDetail
    {
        [Key]
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        [Display(Name = "Parent Category")]
        public long? ParentCategoryId { get; set; }
        public long? SortOrder { get; set; }
        [NotMapped]
        public bool checkCategory { get; set; }
    }
}
