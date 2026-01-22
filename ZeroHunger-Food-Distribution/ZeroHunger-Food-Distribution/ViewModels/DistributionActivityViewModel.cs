using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZeroHunger_Food_Distribution.ViewModels
{
    public class DistributionActivityViewModel
    {
        public int CollectRequestId { get; set; }

        [Required(ErrorMessage = "Distribution date and time is required")]
        [Display(Name = "Distribution Date & Time")]
        public DateTime DistributionTime { get; set; }

        [Required(ErrorMessage = "Distribution location is required")]
        [StringLength(300, ErrorMessage = "Location cannot exceed 300 characters")]
        [Display(Name = "Distribution Location")]
        public string DistributionLocation { get; set; }

        [Required(ErrorMessage = "Number of beneficiaries is required")]
        [Range(1, 10000, ErrorMessage = "Number must be between 1 and 10,000")]
        [Display(Name = "Number of Beneficiaries Served")]
        public int BeneficiariesServed { get; set; }

        [Required(ErrorMessage = "Please describe the distribution")]
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        [Display(Name = "Distribution Notes")]
        public string DistributionNotes { get; set; }

        public DistributionActivityViewModel()
        {
            DistributionTime = DateTime.Now;
        }
    }
}