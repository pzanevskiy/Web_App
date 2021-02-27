using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task5.Models.Filters
{
    public class CustomerFilter
    {
        [Display(Name = "Имя")]
        public string NickName { get; set; }
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
    }
}