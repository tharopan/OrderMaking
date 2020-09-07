using System;

namespace OrderMaking.Models.BusinessEntities
{
    public class ShoppingCartFlat
    {
        public long ProductId { get; set; }

        public long ProductCategoryId { get; set; }

        public string ProductName { get; set; }

        public string ProductCategoryName { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductSize { get; set; }

        public int CustomOrder { get; set; }

        public string Barcode { get; set; }

        public int NumberOfItems { get; set; }

        public DateTime OrderDate { get; set; }

    }
}
