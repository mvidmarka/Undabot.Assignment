using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Undabot.Assignment.Common.BindingModels;

namespace Undabot.Assignment.Common.Interfaces
{
    public interface IProductService
    {
      
        /// <summary>
        /// This method returns filtered items
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<FilterResultBindingModel> GetFilterResultsAsync(FilterBindingModel filter);

        double GetMaxPrice(IEnumerable<ProductBindingModel> products);
        double GetMinPrice(IEnumerable<ProductBindingModel> products);
        string[] GetProductSizes(IEnumerable<ProductBindingModel> products);
        string[] GetDescriptionMostCommonWords(IEnumerable<ProductBindingModel> products);
    }
}
