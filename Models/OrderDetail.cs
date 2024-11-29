using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proiect_ProgramareAvansataPePlatforma.NET.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }

        public virtual Order Order { get; set; }
        public virtual Book Book { get; set; }
    }
}