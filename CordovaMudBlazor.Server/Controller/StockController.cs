using Microsoft.AspNetCore.Mvc;
using CordovaMudBlazor.Shared;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CordovaMudBlazor.Server
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [AllowCORS]
    [DisableRequestSizeLimit]
    public class StockController : ControllerBase
    {
        public StockController()
        {
        }

        private static Stock[] _stock = null;
        public Stock[] GetStocks(int page)
        {
            if (_stock != null) return _stock;
            ProductController pc = new ProductController();
            Product[] prods = pc.GetProducts(1);

            List<Stock> result = new List<Stock>();
            Random rnd = new Random();
            for (int i=0; i<100; i++)
            {
                Stock s = new Stock();
                s.Id = i;
                s.ProductId = rnd.Next(prods.Length);
                s.Quantity = rnd.Next(15);
                s.Product = prods[s.ProductId];
                result.Add(s);
            }

            _stock = result.ToArray();
            return _stock;
        }
    }
}
