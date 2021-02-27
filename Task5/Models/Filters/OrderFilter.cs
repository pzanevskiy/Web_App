using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task5.Models.Filters
{
    public class OrderFilter
    {
        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Display(Name = "Покупатель")]
        public string Customer { get; set; }

        [Display(Name = "Продукт")]
        public string Product { get; set; }

        [Display(Name = "Менеджер")]
        public string Manager { get; set; }
    }
}