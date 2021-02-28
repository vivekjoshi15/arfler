using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Arfler.Models
{
    public class ReservationDetail
    {
        [Key]
        public long reservationId { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string reservationEmail { get; set; }

        public string reservationPhone { get; set; }

        public long guestNum { get; set; }

        public string reservationDate { get; set; }

        public string reservationTime { get; set; }

        public string reservationType { get; set; }

        public DateTime? createdDate { get; set; }

        public long restaurantId { get; set; }
    }
}
