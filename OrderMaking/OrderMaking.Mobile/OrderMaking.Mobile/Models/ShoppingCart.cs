using System;
using System.Collections.Generic;
using System.Text;

namespace OrderMaking.Mobile.Models
{
    public class ShoppingCart
    {
        public string Barcode { get; set; }

        public int CategoryId { get; set; }

        public DateTime OrderDate { get; set; }

        public string ItemDescription { get; set; }

        public int NumberOfItems { get; set; }
    }
}
