using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web1.Code
{
    public class RandomSaleRepository
    {
        private static RandomSaleRepository _repository;
        private static readonly object syncRoot = new object();
        private ConcurrentBag<RandomSale> sales = new ConcurrentBag<RandomSale>();

        private RandomSaleRepository()
        {

        }

        public static RandomSaleRepository GetInstance()
        {
            if(_repository == null)
            {
                lock(syncRoot)
                {
                    if(_repository == null)
                    {
                        _repository = new RandomSaleRepository();
                    }
                }
            }

            return _repository;
        }

        public bool HasUser(User randomUser)
        {
            return sales.Any(sale => sale.User.Md5 == randomUser.Md5);
        }

        public IEnumerable<RandomSale> GetAllSales()
        {
            var query = from s in sales
                        orderby s.CreatedDate descending
                        select s;

            return query;          
        }

        public void AddRandomSale(RandomSale sale)
        {
            this.sales.Add(sale);
        }
    }
}