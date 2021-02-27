using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task5.Models.Customer
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Введите значение")]
        [Display(Name = "Имя")]
        [RegularExpression(@"^[A-Za-z]{2,50}", ErrorMessage = "Неверное занчение. Имя должно содержать только буквы")]
        [StringLength(50,MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 50 символов")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Введите значение")]
        [Display(Name = "Номер телефона")]
        [RegularExpression(@"^(\+375|80)(29|25|44|33)(\d{3})(\d{2})(\d{2})$",ErrorMessage ="Неправильный номер. Пример: +375291234567")]
        public string PhoneNumber { get; set; }
    }
}