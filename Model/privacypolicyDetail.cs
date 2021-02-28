using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arfler.Models
{
    public class privacyPolicies
    {
        [Key]
        public long Id { get; set; }
        public long restaurantId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime modifiedDate { get; set; }
    }
}
