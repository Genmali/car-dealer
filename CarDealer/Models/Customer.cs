using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealer.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required, RegularExpression(@"^[A-Z]+[a-z A-Z]*$")]
        public string Name { get; set; }

        [Required, RegularExpression(@"^[A-Z]+[a-z A-Z]*$")]
        public string Surname { get; set; }

        [Required, Column(TypeName = "int")]
        public int Age { get; set; }

        [Required, RegularExpression(@"^[A-Z]+[a-z A-Z0-9]*$")]
        public string Address { get; set; }

        [Required]
        public bool Created { get; set; }
    }
}
