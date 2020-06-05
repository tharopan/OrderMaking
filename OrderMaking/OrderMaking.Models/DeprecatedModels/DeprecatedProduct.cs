using System.ComponentModel.DataAnnotations.Schema;

namespace OrderMaking.Models
{
    public class DeprecatedProduct
    {
        public long Id { get; set; }

        public string ItemLookup { get; set; }

        public string Description { get; set; }

        public string BarCode { get; set; }

        [ForeignKey("DepartmentId")]
        public DeprecatedDepartment Department { get; set; }

        public int DepartmentId { get; set; }

        public decimal Price { get; set; }

        public string SizeGroup { get; set; }

        [NotMapped]
        public string BarCodeImageBase64 { get; set; }
    }
}
