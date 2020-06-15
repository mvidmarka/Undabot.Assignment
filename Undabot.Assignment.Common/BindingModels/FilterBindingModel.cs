using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Undabot.Assignment.Common.BindingModels
{
  
    public class FilterBindingModel
    {
        [Range(0, 9999)] //validation on input parameters for security reasons 
        public double? MaxPrice { get; set; }

        [StringLength(maximumLength: 80, MinimumLength = 3)] 
        public string Size { get; set; }

        [StringLength(maximumLength: 80, MinimumLength = 3)]
        public string Highlight { get; set; }

        public bool HasFilter => (MaxPrice.HasValue || !string.IsNullOrEmpty(Size) || !string.IsNullOrEmpty(Highlight));
 
    }
}
