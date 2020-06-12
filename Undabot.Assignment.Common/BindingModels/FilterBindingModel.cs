using System;
using System.Collections.Generic;
using System.Text;

namespace Undabot.Assignment.Common.BindingModels
{
    public class FilterBindingModel
    {
        public double? MaxPrice { get; set; }
        public string Size { get; set; }
        public string Highlight { get; set; }

        public bool HasFilter
        {
            get
            {
                return (MaxPrice.HasValue || !string.IsNullOrEmpty(Size) || !string.IsNullOrEmpty(Highlight));
            }
        }

    }
}
