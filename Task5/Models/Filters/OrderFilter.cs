using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task5.Models.Filters
{
    public class OrderFilter
    {
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Display(Name = "Customer")]
        public string Customer { get; set; }

        [Display(Name = "Product")]
        public string Product { get; set; }

        [Display(Name = "Manager")]
        public string Manager { get; set; }
    }
}