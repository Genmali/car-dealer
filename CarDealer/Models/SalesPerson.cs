using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealer.Models
{
    public class SalesPerson
    {
        public int SalesPersonId { get; set; }

        [Required, RegularExpression(@"^[A-Z]+[a-z A-Z]*$")]
        public string Name { get; set; }

        [Required, Display(Name = "Job Title"), RegularExpression(@"^[A-Z]+[a-z A-Z]*$")]
        public string JobTitle { get; set; }

        [Required, RegularExpression(@"^[A-Z]+[a-z A-Z0-9]*$")]
        public string Address { get; set; }

        [Required, DataType(DataType.Currency), Column(TypeName = "decimal(18, 2)")]
        public decimal Salary { get; set; }
    }
}
