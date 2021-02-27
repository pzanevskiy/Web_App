using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task5.Models.Filters
{
    public class ManagerFilter
    {
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Рейтинг")]
        [Range(0, 10, ErrorMessage = "Рейтинг должен быть в диапозоне от 0 о 10")]
        public double? Rating { get; set; }
    }
}