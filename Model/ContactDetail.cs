using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Arfler.Models
{
    public class ContactDetails
    {
        [Key]
        public long contactId { get; set; }

        public string contactMessage { get; set; }

        public string contactEmail { get; set; }

        public DateTime? createdDate { get; set; }

        public string contactName { get; set; }

        public long restaurantId { get; set; }

        public string cSubject { get; set; }

        [NotMapped]
        public string RestaurantName { get; set; }
    }
}
