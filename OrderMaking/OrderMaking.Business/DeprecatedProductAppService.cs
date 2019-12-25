using OrderMaking.Data;
using OrderMaking.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderMaking.Business
{
    public class DeprecatedProductAppService
    {
        Repository<DeprecatedProduct> repository;

        public DeprecatedProductAppService()
        {
            repository = new Repository<DeprecatedProduct>();
        }

        public DeprecatedProduct Get(string barCode)
        {
            var prod = repository.GetByProp(x => x.BarCode == barCode);
            return prod;
        }
    }
}
