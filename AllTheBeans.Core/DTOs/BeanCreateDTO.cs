using System;
using System.Collections.Generic;
using System.Text;

namespace AllTheBeans.Core.DTOs
{
    public class BeanCreateDTO
    {
        public decimal CostPer100g { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Colour { get; set; }
        public string Aroma { get; set; }
    }
}
