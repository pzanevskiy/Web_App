using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task5.Models.Filters
{
    public class ProductFilter
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Price")]
        [Min(1)]
        public double? Price { get; set; }
    }
}