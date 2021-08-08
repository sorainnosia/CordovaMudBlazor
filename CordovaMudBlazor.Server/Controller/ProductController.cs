using Microsoft.AspNetCore.Mvc;
using CordovaMudBlazor.Shared;
using System.Collections.Generic;

namespace CordovaMudBlazor.Server
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [AllowCORS]
    [DisableRequestSizeLimit]
    public class ProductController : ControllerBase
    {
        public ProductController()
        {
        }

        public Product[] GetProducts(int page)
        {
            List<Product> result = new List<Product>();
            result.Add(new Product { Id = 1, Name = "Lifebuoy Hand Sanitizer Total10", Price = 2.2M });
            result.Add(new Product { Id = 2, Name = "Everfresh Antibacterial Wipes", Price = 1.5M });
            result.Add(new Product { Id = 3, Name = "Seasons Ice Lemon Tea 500ml", Price = 1.2M });
            result.Add(new Product { Id = 4, Name = "Pokka Green Tea 500ml", Price = 1M });
            result.Add(new Product { Id = 5, Name = "Ice Mountain Pure Drinking Water 500ml", Price = 0.8M });
            result.Add(new Product { Id = 6, Name = "JacknJill Potato Chip 60g", Price = 1M });
            result.Add(new Product { Id = 7, Name = "Mei Ka Ni Disposable Mask 50 ply", Price = 4M });
            result.Add(new Product { Id = 8, Name = "Kodak AAA Batteris x10", Price = 2M });
            result.Add(new Product { Id = 9, Name = "Panadol E215 20 Caplets", Price = 6M });
            result.Add(new Product { Id = 10, Name = "Panadol Cough & Flu 16 Caplets", Price = 6M });
            result.Add(new Product { Id = 11, Name = "Sampoerna A Menthol", Price = 13.4M });
            result.Add(new Product { Id = 12, Name = "Dettol Disinfectant Spray", Price = 4M });
            result.Add(new Product { Id = 13, Name = "Dettol Disinfectant Liquid 1L", Price = 5M });
            result.Add(new Product { Id = 14, Name = "Baygon Dual Action 500ml", Price = 6M });

            return result.ToArray();
        }
    }
}
