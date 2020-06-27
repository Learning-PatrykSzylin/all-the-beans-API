using System;
using System.Collections.Generic;
using System.Text;

namespace AllTheBeans.Core.Models
{
    public class Bean
    {
        public int BeanId { get; set; }
        public decimal CostPer100g { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Colour { get; set; }
        public string Aroma { get; set; }

    }
}
