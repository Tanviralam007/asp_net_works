using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZeroHunger_Food_Distribution.ViewModels
{
    public class CollectionActivityViewModel
    {
        public int CollectRequestId { get; set; }

        [Required(ErrorMessage = "Collection date and time is required")]
        [Display(Name = "Collection Date & Time")]
        public DateTime CollectionTime { get; set; }

        [Required(ErrorMessage = "Please describe the food condition")]
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        [Display(Name = "Food Condition & Notes")]
        public string CollectionNotes { get; set; }

        [Required(ErrorMessage = "Collected quantity is required")]
        [StringLength(100)]
        [Display(Name = "Actual Quantity Collected")]
        public string ActualQuantity { get; set; }

        public CollectionActivityViewModel()
        {
            CollectionTime = DateTime.Now;
        }
    }
}