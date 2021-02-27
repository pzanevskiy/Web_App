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
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Цена")]
        [Min(1, ErrorMessage = "Значение не должно быть меньше 1")]
        public double? Price { get; set; }
    }
}