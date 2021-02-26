using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task5.Models.Order
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter value")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Enter value")]
        [Min(1)]
        public double Price { get; set; }

        [Required(ErrorMessage = "Enter value")]
        public string Customer { get; set; }
        [Required(ErrorMessage = "Enter value")]
        public string Product { get; set; }
        [Required(ErrorMessage = "Enter value")]
        public string Manager { get; set; }
    }
}