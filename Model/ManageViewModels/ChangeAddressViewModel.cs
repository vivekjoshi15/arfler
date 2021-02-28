using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Arfler.Models.ManageViewModels
{
    public class ChangeAddressViewModel
    {
        [Required]
       
        [Display(Name = "Address")]
        public string Address1 { get; set; }
    }
}
