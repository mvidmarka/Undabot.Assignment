using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Undabot.Assignment.Common.BindingModels
{
    public class FilterResultBindingModel
    {
        [JsonProperty("products")]
        public IEnumerable<ProductBindingModel> Products { get; set; }

        [JsonProperty("min_price")]
        public double MinPrice { get; set; }

        [JsonProperty("max_price")]
        public double MaxPrice { get; set; }

        [JsonProperty("sizes")]
        public string[] Sizes { get; set; }

        [JsonProperty("commonwords")]
        public string[] CommonWords { get; set; }


    }
}
