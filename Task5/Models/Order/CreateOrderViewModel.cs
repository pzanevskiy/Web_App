using DataAnnotationsExtensions;
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
        [Required(ErrorMessage = "Введите значение")]
        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Введите значение")]
        [Display(Name = "Цена")]
        [Min(1, ErrorMessage = "Значение не должно быть меньше 1")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Выберите значение")]
        [Display(Name = "Покупатель")]
        public string Customer { get; set; }

        [Required(ErrorMessage = "Выберите значение")]
        [Display(Name = "Продукт")]
        public string Product { get; set; }

        [Required(ErrorMessage = "Выберите значение")]
        [Display(Name = "Менеджер")]
        public string Manager { get; set; }

        public SelectList Customers;
        public SelectList Products;
        public SelectList Managers;
    }
}