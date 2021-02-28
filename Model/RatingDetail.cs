using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arfler.Models
{
    public class RatingDetail
    {
        [Key]
        public long rateId { get; set; }
        public long restaurantId { get; set; }
        public long rating { get; set; }
        public DateTime? createdDate { get; set; }
    }
}
