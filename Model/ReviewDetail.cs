using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arfler.Models
{
    public class ReviewDetail
    {
        [Key]
        public long reviewId { get; set; }
        public string review { get; set; }
        public DateTime? createdDate { get; set; }
        public long restaurantId { get; set; }
        public long reviewUserId { get; set; }

    }
}
