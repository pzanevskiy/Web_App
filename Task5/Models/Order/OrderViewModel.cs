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

        [Required(ErrorMessage = "Введите значение")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Введите значение")]
        [Min(1, ErrorMessage = "Значение не должно быть меньше 1")]
        [Display(Name = "Цена")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Введите значение")]
        [Display(Name = "Покупатель")]
        public string Customer { get; set; }
        [Required(ErrorMessage = "Введите значение")]
        [Display(Name = "Продукт")]
        public string Product { get; set; }
        [Required(ErrorMessage = "Введите значение")]
        [Display(Name = "Менеджер")]
        public string Manager { get; set; }
    }
}