using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class DayComplexViewModel
    {
        public DateTime Date { get; set; }

        public int ComplexId { get; set; }
        public string ComplexName { get; set; }


        public int CompanyId { get; set; }
        public bool Enabled { get; set; }

        public int CategoryId { get; set; }

        public int? PictureId { get; set; }
    }


}
