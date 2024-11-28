using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proiect_ProgramareAvansataPePlatforma.NET.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}