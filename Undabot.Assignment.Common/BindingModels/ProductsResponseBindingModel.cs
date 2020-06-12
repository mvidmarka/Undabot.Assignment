using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Undabot.Assignment.Common.BindingModels
{
    public class ProductsResponseBindingModel
    {
        [JsonProperty("products")]
        public ProductBindingModel[] Products { get; set; }

        [JsonProperty("apiKeys")]
        public Apikeys ApiKeys { get; set; }
    }


    public class Apikeys
    {
        [JsonProperty("primary")]
        public string Primary { get; set; }

        [JsonProperty("secondary")]
        public string Secondary { get; set; }
    }

}
