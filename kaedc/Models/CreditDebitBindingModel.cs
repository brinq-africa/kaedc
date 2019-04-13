using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace kaedc.Models
{
    public class CreditDebitBindingModel
    {
        [Required]
        [Display(Name = "User's Account Number")]
        public string BrinqAccountNumber { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public string Amount { get; set; }

        [Required]
        public int serviceId { get; set; } 
    }
}
