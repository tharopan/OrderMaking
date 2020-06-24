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
                    var existingShoppingCart = repository.Get(
                        x => x.ProductId == product.Id &&
                        x.CategoryId == shoppingCart.CategoryId
                        ).FirstOrDefault();

                    if (existingShoppingCart == null)
                    {
                        shoppingCart.ProductId = product.Id;
                        repository.Insert(shoppingCart);
                        repository.Save();
                    }
                }
                else
                {
                    //var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                    //{
                    //    Content = new StringContent(string.Format("No product with ID = {0}", shoppingCart.Barcode),
                    //    ReasonPhrase = "Product ID Not Found"
                    //};
                    //throw new HttpResponseException(resp);
                }
            }
            else if (shoppingCart.ProductId > 0)
            {
                product = productRepository.GetById(shoppingCart.ProductId);
                if (product != null)
                {
                    var existingShoppingCart = repository.Get(
                        x => x.ProductId == product.Id &&
                        x.CategoryId == shoppingCart.CategoryId
                        ).FirstOrDefault();

                    if (existingShoppingCart == null)
                    {
                        shoppingCart.ProductId = product.Id;
                        repository.Insert(shoppingCart);
                        repository.Save();
                    }
                }
            }
        }

        public void Remove(string barcode)
        {
            if (!string.IsNullOrEmpty(barcode))
            {
                var shopppingCartTobeRemove = repository.Get(x => x.Product.BarCode == barcode).FirstOrDefault();
                if (shopppingCartTobeRemove != null)
                {
                    repository.Delete(shopppingCartTobeRemove);
                    repository.Save();
                }
            }
        }
    }
}