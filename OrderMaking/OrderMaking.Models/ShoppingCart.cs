using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderMaking.Models
{
    public class ShoppingCart : Model
    {
        public DateTime OrderDate { get; set; }

        public int CategoryId { get; set; }

        public DeprecatedCategory Category { get; set; }

        public long ProductId { get; set; }

        [NotMapped]
        public string Barcode { get; set; }

        public DeprecatedProduct Product { get; set; }

        public int NumberOfItems { get; set; }
    }
}
