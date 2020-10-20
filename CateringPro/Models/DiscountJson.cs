using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class DiscountJson
    {
        public List<int> CategoriesId { get; set; }
        public DiscountJson()
        {
            CategoriesId = new List<int>();
        }
    }
}
