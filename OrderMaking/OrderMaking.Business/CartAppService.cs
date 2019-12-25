using OrderMaking.Data;
using OrderMaking.Models;
using System.Linq;

namespace OrderMaking.Business
{
    public class CartAppService
    {
        Repository<ShoppingCart> repository;
        Repository<DeprecatedProduct> productRepository;

        public CartAppService()
        {
            repository = new Repository<ShoppingCart>();
            productRepository = new Repository<DeprecatedProduct>();
        }

        public void Add(ShoppingCart shoppingCart)
        {
            DeprecatedProduct product;
            if (!string.IsNullOrEmpty(shoppingCart.Barcode))
            {
                product = productRepository.Get(x => x.BarCode == shoppingCart.Barcode).FirstOrDefault();
                if (product != null)
                {
                    shoppingCart.ProductId = product.Id;
                    repository.Insert(shoppingCart);
                    repository.Save();
                }
            }
            else if (shoppingCart.ProductId > 0)
            {
                product = productRepository.GetById(shoppingCart.ProductId);
                if (product != null)
                {
                    shoppingCart.ProductId = product.Id;
                    repository.Insert(shoppingCart);
                    repository.Save();
                }
            }
        }
    }
}
