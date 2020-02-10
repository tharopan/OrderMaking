using System;
using System.Collections.Generic;
using System.Text;

namespace OrderMaking.Models.DataMigration
{
    public class ProductFromJson
    {
        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public string ProductLink { get; set; }

        public string Code { get; set; }

        public string Size { get; set; }

        public string Price { get; set; }
    }
}
