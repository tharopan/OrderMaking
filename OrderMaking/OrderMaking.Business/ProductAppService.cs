using OrderMaking.Data;
using OrderMaking.Models;

namespace OrderMaking.Business
{
    public class ProductAppService
    {

        Repository<Product> repository;

        public ProductAppService()
        {
            repository = new Repository<Product>();
        }

        public Product Get(string barCode)
        {
            var prod = repository.GetByProp(x => x.BarCode == barCode);
            return prod;
        }
    }
}
