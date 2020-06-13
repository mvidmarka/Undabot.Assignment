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
    /// <summary>
    /// Test class for product service 
    /// I have added some unit tests for example not all methods from service are in the test
    /// </summary>
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

            //mock some dummy data
            _productItems = new List<ProductBindingModel>()
            {
                new ProductBindingModel() {Price = 5, Description = "This trouser perfectly pairs with a green shirt.", Sizes = new List<string>{  "small", "medium", "large" }.ToArray(), Title = "A Red Trouser" },
                new ProductBindingModel() {Price = 15, Description = "This trouser perfectly pairs with a blue shirt.", Sizes = new List<string>{ "small" }.ToArray(), Title = "A Blue Trouser" },
                new ProductBindingModel() {Price = 25, Description = "This trouser perfectly pairs with a green shirt.", Sizes = new List<string>{ "large" }.ToArray(), Title = "A Red Trouser" },
                new ProductBindingModel() {Price = 30, Description = "This trouser perfectly pairs with a blue shirt.", Sizes = new List<string>{ "small", "medium" }.ToArray(), Title = "A Green Trouser" },
            };

        }

        [TestMethod]
        public void GetProductMaxPriceTest()
        {
            var expected = 30;
            var result = _productService.GetMaxPrice(_productItems);
            Assert.AreEqual(expected, result, "Max price is not calculating correctly");
        }

        [TestMethod]
        public void GetProductMinPriceTest()
        {
            var expected = 5;
            var result = _productService.GetMinPrice(_productItems);
            Assert.AreEqual(expected, result, "Min price is not calculating correctly");
        }

        [TestMethod]
        public void GetDescriptionMostCommonWordsTest()
        {
            string[] expected = new[] { "This", "trouser", "green", "blue" };
            var result = _productService.GetDescriptionMostCommonWords(_productItems);

            Assert.IsNotNull(result, "Method returned null");
            Assert.AreEqual(expected.Length, result.Length, "Not returning exprected count");
            Assert.AreEqual(expected[0], result[0], "Common keywords are not same");
            Assert.AreEqual(expected[1], result[1], "Common keywords are not same");
            Assert.AreEqual(expected[2], result[2], "Common keywords are not same");
            Assert.AreEqual(expected[3], result[3], "Common keywords are not same");

        }

    }
}
