using DataAnnotationsExtensions;
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
        [Required(ErrorMessage = "Введите значение")]
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введите значение")]
        [Min(1,ErrorMessage ="Значение не должно быть меньше 1")]
        [Display(Name = "Цена")]
        public double? Price { get; set; }
    }
}