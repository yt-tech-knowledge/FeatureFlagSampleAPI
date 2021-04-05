using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace FeatureFlagSampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private static readonly string[] Products = new[]
        {
            "Book", "Pencil", "Notebook", "Backpack", "Pen"
        };

        private readonly ILogger<ProductController> _logger;
        private readonly IFeatureManager _featureManager;

        public ProductController(ILogger<ProductController> logger, IFeatureManager featureManager)
        {
            _logger = logger;
            _featureManager = featureManager;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            var rng = new Random();

            var addProductPrice = await _featureManager.IsEnabledAsync(nameof(FeatureFlag.AddProductPrice));

            return Enumerable.Range(1, Products.Length).Select(index => new Product
            {
                Price = addProductPrice ? Math.Round(rng.NextDouble(), 2) : 0,
                Name = Products[index - 1]

            }).ToArray();
        }
    }
}
