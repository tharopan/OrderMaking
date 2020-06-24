using OrderMaking.Data;
using OrderMaking.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderMaking.Business
{
    public class InternalAppService
    {
        Repository<ShoppingCart> repository;


        public InternalAppService()
        {
            repository = new Repository<ShoppingCart>();
        }

        public void SyncDb()
        {
            repository.Execute("SyncDb");
        }
    }
}
