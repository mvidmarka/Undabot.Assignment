using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Undabot.Assignment.Common.BindingModels
{

    public class ProductBindingModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("sizes")]
        public string[] Sizes { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

}
