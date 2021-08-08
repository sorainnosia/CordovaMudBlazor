using CordovaMudBlazor.Shared;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CordovaMudBlazor.Sample
{
    public class SaleController
    {
        public SaleController()
        {
        }

        private static Sale[] _sale = null;
        public Sale[] GetSales(int page)
        {
            if (_sale != null) return _sale;

            ProductController pc = new ProductController();
            Product[] prods = pc.GetProducts(1);
            List<Sale> result = new List<Sale>();

            Random rnd = new Random();
            for (int i=0; i<100;i++)
            {
                Sale s = new Sale();
                int c = rnd.Next(prods.Length);
                s.ProductId = c;
                s.Price = prods[c].Price;
                s.Quantity = rnd.Next(3) + 1;
                s.Id = i;
                s.Date = new DateTime(2021, rnd.Next(12) + 1, (rnd.Next(28) + 1));
                s.Product = prods[c];
                result.Add(s);
            }
            
            _sale = result.ToArray();
            return _sale;
        }
    }
}
