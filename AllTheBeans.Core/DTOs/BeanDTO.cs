using System;
using System.Collections.Generic;
using System.Text;

namespace AllTheBeans.Core.DTOs
{
    public class BeanDTO
    {
        public decimal CostPer100g { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Colour { get; set; }
        public string Aroma { get; set; }

        public bool IsValid()
        {
            return Name != null && Name.Length > 0;
        }
    }
}
