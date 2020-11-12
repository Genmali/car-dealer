using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealer.Models
{
    public class Car
    {
        public int CarId { get; set; }

        [Required, RegularExpression(@"^[A-Z]+[a-z A-Z0-9]*$")]
        public string Make { get; set; }

        [Required, RegularExpression(@"^[A-Z]+[a-z A-Z0-9]*$")]
        public string Model { get; set; }

        [Required, RegularExpression(@"^[A-Z]+[a-z A-Z]*$")]
        public string Color { get; set; }

        [Required, RegularExpression(@"^[A-Z]+[a-z A-Z]*$")]
        public string Extras { get; set; }

        [Required, Display(Name = "Recommended Price"), DataType(DataType.Currency), Column(TypeName = "decimal(18, 2)")]
        public decimal RecommendedPrice { get; set; }
    }
}
