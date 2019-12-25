using System;
using System.Collections.Generic;
using System.Text;

namespace OrderMaking.Models
{
    public class ShoppingCartDeprecated
    {
        public DateTime OrderDate { get; set; }

        public long ProductId { get; set; }

        public int DepartmentId { get; set; }

        public int NumberOfItems { get; set; }
    }
}
