using OrderMaking.Data;
using OrderMaking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderMaking.Business
{
    public class CategoryAppService
    {
        Repository<DeprecatedCategory> repository;

        public CategoryAppService()
        {
            repository = new Repository<DeprecatedCategory>();
        }

        public IList<DeprecatedCategory> Get()
        {
            return repository.Get().ToList();
        }
    }
}
