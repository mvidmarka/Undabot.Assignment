using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using Undabot.Assignment.Common.BindingModels;
using Undabot.Assignment.Common.Interfaces;
using Undabot.Assignment.Common.Utils;
using Undabot.Assignment.Core.Services;

namespace Undabot.Assignment.Test
{
    [TestClass]
    public class ProductServiceTests
    {
        private readonly ILogger<ProductService> _logger = Substitute.For<ILogger<ProductService>>();
        private readonly HttpClientHelper _httpClientHelper = Substitute.For<HttpClientHelper>();
        private readonly List<ProductBindingModel> _productItems = new List<ProductBindingModel>();
        private readonly IProductService _productService;

        public ProductServiceTests()
        {
            _productService = new ProductService(_httpClientHelper, _logger);

            //setup some dummy data
            _productItems = new List<ProductBindingModel>()
            {
                new ProductBindingModel() {Price = 5},
                new ProductBindingModel() {Price = 15},
                new ProductBindingModel() {Price = 25},
                new ProductBindingModel() {Price = 30}
            };

        }


        [TestMethod]
        public void GetProductMaxPrice()
        {
          
            var expected = 30;
            var result = _productService.GetMaxPrice(_productItems);
            Assert.AreEqual(expected, result, "Max price is not calculating correctly");
        }


        [TestMethod]
        public void GetProductMinPrice()
        {
            var expected = 5;
            var result = _productService.GetMinPrice(_productItems);
            Assert.AreEqual(expected, result, "Min price is not calculating correctly");
        }

        //TODO add other tests, I have added only this tests as an example i guess there is no need for more this is an example 
    }
}
