using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiect_ProgramareAvansataPePlatforma.NET.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public string BookTitle { get; set; }
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Stocul cărții alese trebuie să fie un număr pozitiv mai mare ca 0.")]
        public int Quantity { get; set; }   
        public virtual Order Order { get; set; }
        public virtual Book Book { get; set; }
    }
}