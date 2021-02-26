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
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Customer { get; set; }

        [Required]
        public string Product { get; set; }

        [Required]
        public string Manager { get; set; }

        public SelectList Customers;
        public SelectList Products;
        public SelectList Managers;
    }
}