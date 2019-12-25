namespace OrderMaking.Models
{
    public class Product : Model
    {
        public string Name { get; set; }

        public string BarCode { get; set; }

        //public Category Category { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public string Description { get; set; }

        public string ProductCode { get; set; }

        public string Size { get; set; }

        public int CustomOrder { get; set; }
    }
}
