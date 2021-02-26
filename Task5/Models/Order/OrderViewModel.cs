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
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string Customer { get; set; }
        public string Product { get; set; }
        public string Manager { get; set; }
    }
}