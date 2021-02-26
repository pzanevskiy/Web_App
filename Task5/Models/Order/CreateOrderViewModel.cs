using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Task5.Models.Order
{
    public class CreateOrderViewModel
    {
        [Required(ErrorMessage ="Enter value")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Enter value")]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Select value")]
        public string Customer { get; set; }

        [Required(ErrorMessage = "Select value")]
        public string Product { get; set; }

        [Required(ErrorMessage = "Select value")]
        public string Manager { get; set; }

        public SelectList Customers;
        public SelectList Products;
        public SelectList Managers;
    }
}