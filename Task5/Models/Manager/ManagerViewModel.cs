using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task5.Models.Manager
{
    public class ManagerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите значение")]
        [Display(Name = "Фамилия")]
        [RegularExpression(@"^[A-Za-z]{2,50}")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Строка должна быть от 2 до 50 символов")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите значение")]
        [Display(Name = "Рейтинг")]
        [Range(0,10,ErrorMessage ="Рейтинг должен быть в диапозоне от 0 о 10")]
        public double Rating { get; set; }
    }
}