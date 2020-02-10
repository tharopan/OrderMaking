using Newtonsoft.Json;
using OrderMaking.Models;
using OrderMaking.Models.DataMigration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OrderMaking.DataMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            int parentCatId = 4;
            int childCatId = 5;
            string jsonFileName = "";
            
            var program = new Program();
            var products = program.ReadJson(jsonFileName);
            program.DownloadImages(products, parentCatId, childCatId);
            program.InsertProducts(products, parentCatId, childCatId);

            Console.ReadLine();
        }

        public IList<ProductFromJson> ReadJson(string jsonFileName)
        {
            List<JsonProduct> items = new List<JsonProduct>();
            //TODO : Read json data
            using (StreamReader r = new StreamReader(jsonFileName))
            {
                string json = r.ReadToEnd();
                 items = JsonConvert.DeserializeObject<List<JsonProduct>>(json);
            }

            return items.Select(x => new ProductFromJson()
            {
                Code = x.Field3,
                ImageUrl = x.Field1,
                Name = x.Field2_Text,
                Price = x.Field4,
                ProductLink = x.Field2_Link,
                Size = x.Field5
            }).ToList();
        }

        public void DownloadImages(IList<ProductFromJson> products, int parentCatId, int childCatId)
        {
            if(products == null || !products.Any())
            {
                return;
            }

            var rootPath = $@"C:\ProductImages\{parentCatId}\{childCatId}";

            foreach (var product in products)
            {
                if(product != null)
                {
                    //TODO Download Image

                    System.IO.Directory.CreateDirectory(rootPath);

                    //TODO Save Image in the root path with image ID

                }
            }
        }

        public void InsertProducts(IList<ProductFromJson> products, int parentCatId, int childCatId)
        {
            if (products == null || !products.Any())
            {
                return;
            }

            List<Product> productList = products.Select(x => new Product() 
            { 
                Name = x.Name,
                ProductCode = x.Code,
                Image = x.ImageUrl,
                Price = !string.IsNullOrEmpty(x.Price) ? Convert.ToDecimal(x.Price.Substring(5)): 0,
                Size = x.Size
            }).ToList();

            //TODO insert the product as a bulk
        }
    }
}
