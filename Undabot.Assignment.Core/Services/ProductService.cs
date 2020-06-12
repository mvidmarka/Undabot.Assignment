using System;
using System.Collections.Generic;
using System.Text;
using Undabot.Assignment.Common.BindingModels;
using Undabot.Assignment.Common.Interfaces;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Undabot.Assignment.Common.Utils;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using System.Linq;

namespace Undabot.Assignment.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClientHelper _httpClientHelper;
        ILogger<ProductService> _logger;

        public ProductService(HttpClientHelper httpClientHelper, ILogger<ProductService> logger)
        {
            _httpClientHelper = httpClientHelper;
            _logger = logger;
   
        }

        private async Task<IEnumerable<ProductBindingModel>> GetProductsAsync()
        {

            string json = await _httpClientHelper.GetJsonFromApi("http://www.mocky.io/v2/5e307edf3200005d00858b49");
            ProductsResponseBindingModel resopnse = JsonConvert.DeserializeObject<ProductsResponseBindingModel>(json);
            return resopnse.Products;

        }

        public async Task<FilterResultBindingModel> GetFilterResultsAsync(FilterBindingModel filter)
        {
            IEnumerable<ProductBindingModel> products = await GetProductsAsync();
            FilterResultBindingModel result = new FilterResultBindingModel();
      
            if (filter.HasFilter)
            {
                //Filter products
            }
            else
            {
                result.Products = products;
            }

            result.MaxPrice = GetMaxPrice(products);
            result.MinPrice = GetMinPrice(products);
            result.Sizes = GetProductSizes(products);
            result.CommonWords = GetDescriptionMostCommonWords(products);
            HighlightDescriptions(products, filter.Highlight?.Split(",").ToArray());


            return result;
        }

        public double GetMaxPrice(IEnumerable<ProductBindingModel> products)
        {
           return products.OrderByDescending(x => x.Price).Select(x => x.Price).FirstOrDefault();
        }

        public double GetMinPrice(IEnumerable<ProductBindingModel> products)
        {
            return products.OrderBy(x => x.Price).Select(x => x.Price).FirstOrDefault();
        }

        public string[] GetProductSizes(IEnumerable<ProductBindingModel> products)
        {
            string[] items = products.Select(x => x.Sizes).SelectMany(x => x).Distinct().ToArray();
      
            return items;
        }

        public string[] GetDescriptionMostCommonWords(IEnumerable<ProductBindingModel> products)
        {
            //get list of all words
            var descriptions = products.Select(x => x.Description).ToList().SelectMany(x => x.Split(" "));

            //count and group words
            var count = descriptions
            .GroupBy(n => n)
            .Select(n => new
            {
                Keyword = n.Key,
                Count = n.Count()
            }
            )
            .OrderBy(n => n.Count).ToList();

            //exclude top 5 common and create array of 10 items
            var result = count.Take(count.Count - 5).OrderByDescending(x => x.Count).Select(x => x.Keyword).Take(10).ToArray();
  
            return result;
        }

        public void HighlightDescriptions(IEnumerable<ProductBindingModel> products, string[] highlight)
        {

            products.ToList().ForEach(p => highlight?.ToList().ForEach(y => p.Description = p.Description.Replace(y, $"<em>{y}</em>")));
    
        }
    }
}
