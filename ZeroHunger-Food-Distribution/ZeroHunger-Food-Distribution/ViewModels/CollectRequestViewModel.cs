using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZeroHunger_Food_Distribution.ViewModels
{
    public class CollectRequestViewModel
    {
        [Required(ErrorMessage = "Food description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Food Description")]
        public string FoodDescription { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Display(Name = "Approximate Quantity")]
        [StringLength(100, ErrorMessage = "Quantity description cannot exceed 100 characters")]
        public string ApproximateQuantity { get; set; }
        [Required(ErrorMessage = "Preservation time is required")]
        [Display(Name = "Preserve Until")]
        [DataType(DataType.DateTime)]
        public DateTime PreservationTime { get; set; }
        [StringLength(500, ErrorMessage = "Remarks cannot exceed 500 characters")]
        [Display(Name = "Additional Remarks")]
        public string Remarks { get; set; }
    }
}