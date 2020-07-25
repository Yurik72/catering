using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class ComplexCategory
    {
        public int ComplexId { get; set; }
        public Complex Complex { get; set; }
        public int CategoryId { get; set; }
        public Categories Categories { get; set; }
    }
}
