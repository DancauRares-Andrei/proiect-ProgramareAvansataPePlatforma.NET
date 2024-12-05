using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiect_ProgramareAvansataPePlatforma.NET.Models
{
    public class Book
    {
        [DisplayName("Autorul")]
        [Required(ErrorMessage = "Numele autorului este obligatoriu.")]
        [StringLength(100, ErrorMessage = "Numele autorului nu poate avea mai mult de 100 de caractere.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Numele autorului poate conține doar litere și spații.")]
        public string Author { get; set; }

        [DisplayName("Titlul cărții")]
        [Required(ErrorMessage = "Titlul cărții este obligatoriu.")]
        [Key]
        [StringLength(100, ErrorMessage = "Titlul cărții nu poate avea mai mult de 100 de caractere.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Titlul cărții poate conține doar litere, cifre și spații.")]
        public string BookTitle { get; set; }
        [DisplayName("Preț")]
        [Required(ErrorMessage = "Prețul este obligatoriu.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Prețul trebuie să fie un număr pozitiv cu maximum două zecimale.")]
        public decimal Price { get; set; }
        [DisplayName("Stoc")]
        [Required(ErrorMessage = "Stocul este obligatoriu.")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Stocul trebuie să fie un număr pozitiv mai mare ca 0.")]
        public int Stock { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }

}