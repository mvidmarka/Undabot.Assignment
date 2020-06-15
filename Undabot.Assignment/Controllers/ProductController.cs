using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Undabot.Assignment.Common.BindingModels;
using Undabot.Assignment.Common.Interfaces;

namespace Undabot.Assignment.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }


        [HttpGet]
        [Route("filter")]
        public async Task<FilterResultBindingModel> Filter([FromQuery] FilterBindingModel filter = null)
        {
            var results = await _productService.GetFilterResultsAsync(filter);
            return results;
        }

    }
}