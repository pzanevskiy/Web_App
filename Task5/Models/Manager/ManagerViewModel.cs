using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task5.Models.Manager
{
    public class ManagerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter value")]
        [Display(Name = "Last name")]
        [RegularExpression(@"^[A-Za-z]{2,50}")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "String length must be between 2 and 50")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter value")]
        [Display(Name = "Rating")]
        public double Rating { get; set; }
    }
}