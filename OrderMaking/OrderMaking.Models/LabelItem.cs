using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OrderMaking.Models
{
    public class LabelItem
    {
        public long Id { get; set; }

        public long ProductId { get; set; }

        public DeprecatedProduct Product { get; set; }

        [NotMapped]
        public string Barcode { get; set; }
    }
}
