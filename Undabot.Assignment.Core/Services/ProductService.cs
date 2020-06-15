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
    /// <summary>
    /// Product service all calculations, business logic etc. for product is located here. 
    /// Usally I would put helper methods to separate class and unit test it outside scope of the service that is using them
    /// but for this example I have left them here so I dont waste my time on that. 
    /// Also logger and http client helper is injected with dependancy injection.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly HttpClientHelper _httpClientHelper;
        ILogger<ProductService> _logger;

        public ProductService(HttpClientHelper httpClientHelper, ILogger<ProductService> logger)
        {
            _httpClientHelper = httpClientHelper;
            _logger = logger;

        }

        public async Task<FilterResultBindingModel> GetFilterResultsAsync(FilterBindingModel filter)
        {
            IEnumerable<ProductBindingModel> products = await GetProductsAsync();
            FilterResultBindingModel result = new FilterResultBindingModel();

            if (filter.HasFilter)
            {
                //Filter products
                result.Products = FilterProducts(products, filter);
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

        #region Data access
        private async Task<IEnumerable<ProductBindingModel>> GetProductsAsync()
        {
            try
            {
                string json = await _httpClientHelper.GetJsonFromApi("http://www.mocky.io/v2/5e307edf3200005d00858b49");
                ProductsResponseBindingModel resopnse = JsonConvert.DeserializeObject<ProductsResponseBindingModel>(json);
                _logger.LogInformation("Fetch products success", json); 
                return resopnse.Products;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        #endregion

        #region Helper methods

        public double GetMaxPrice(IEnumerable<ProductBindingModel> products)
        {
            try
            {
                return products.OrderByDescending(x => x.Price).Select(x => x.Price).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }

        }

        public double GetMinPrice(IEnumerable<ProductBindingModel> products)
        {
            try
            {
                return products.OrderBy(x => x.Price).Select(x => x.Price).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public string[] GetProductSizes(IEnumerable<ProductBindingModel> products)
        {
            try
            {
                string[] items = products.Select(x => x.Sizes).SelectMany(x => x).Distinct().ToArray();
                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }

        }

        public string[] GetDescriptionMostCommonWords(IEnumerable<ProductBindingModel> products)
        {

            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public void HighlightDescriptions(IEnumerable<ProductBindingModel> products, string[] highlight)
        {
            try
            {
                products.ToList().ForEach(p => highlight?.ToList().ForEach(y => p.Description = p.Description.Replace(y, $"<em>{y}</em>")));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }

        }

        public IEnumerable<ProductBindingModel> FilterProducts(IEnumerable<ProductBindingModel> products, FilterBindingModel filter)
        {
            try
            {
                IEnumerable<ProductBindingModel> results = new List<ProductBindingModel>();

                string[] sizes = (filter.Size != null) ? filter.Size.Split(",").ToArray() : GetProductSizes(products); //if fileter has no sizes then use all sizes
                double maxPrice = filter.MaxPrice.HasValue ? filter.MaxPrice.Value : GetMaxPrice(products); //check if filter has max price value if not max price is top price from products
                results = products.Where(product => sizes.Any(size => product.Sizes.Contains(size))).Where(p => p.Price <= maxPrice);

                return results;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
        #endregion
    }
}
