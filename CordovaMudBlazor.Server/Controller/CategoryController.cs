using Microsoft.AspNetCore.Mvc;
using CordovaMudBlazor.Shared;
using System.Collections.Generic;

namespace CordovaMudBlazor.Server
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [AllowCORS]
    [DisableRequestSizeLimit]
    public class CategoryController : ControllerBase
    {
        public CategoryController()
        {
        }

        public Category[] GetCategories(int page)
        {
            List<Category> result = new List<Category>();
            result.Add(new Category { Id = 1, Name = "Personal Hygiene" });
            result.Add(new Category { Id = 2, Name = "Health Care" });
            result.Add(new Category { Id = 3, Name = "Food & Beverages" });

            return result.ToArray();
        }
    }
}
