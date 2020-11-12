using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealer.Models
{
    public class CarPurchase
    {
        public int CarPurchaseId { get; set; }

        [Display(Name = "Customer Name")]
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        [Display(Name = "Car Model/Make")]
        public int CarId { get; set; }

        public virtual Car Car { get; set; }

        [Required, Display(Name = "Order Date"), DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        [Required, Display(Name = "Price Paid"), DataType(DataType.Currency), Column(TypeName = "decimal(18, 2)")]
        public decimal PricePaid { get; set; }

        public int SalesPersonId { get; set; }

        [Display(Name = "Sales Person")]
        public virtual SalesPerson SalesPerson { get; set; }
    }
}
