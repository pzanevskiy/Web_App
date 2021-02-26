using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task5.Models.Customer
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Enter value")]
        [Display(Name = "Name")]
        [RegularExpression(@"^[A-Za-z]{2,50}", ErrorMessage = "Incorrect value. The name must contain only letters")]
        [StringLength(50,MinimumLength = 2, ErrorMessage = "String length must be between 2 and 50")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Enter value")]
        [Display(Name = "Phone number")]
        [RegularExpression(@"^(\+375|80)(29|25|44|33)(\d{3})(\d{2})(\d{2})$",ErrorMessage ="Incorrect number. Example: +375291234567")]
        public string PhoneNumber { get; set; }
    }
}