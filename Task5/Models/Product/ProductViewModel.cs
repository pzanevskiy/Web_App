using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task5.Models.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter value")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter value")]
        public double? Price { get; set; }
    }
}