using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class CompanyMenuModel
    {

        public CompanyModel Company { get; set; }
        public IEnumerable<CompanyMenuDayModel> Days { get; set; }

    }
    public class CompanyMenuDayModel
    {
        public DateTime DayDate { get; set; }
        public IEnumerable<CompanyMenuItemModel> Items { get; set; }

        public IEnumerable<CompanyMenuComplexModel> ComplexItems { get; set; }
    }
    public class CompanyMenuComplexModel
    {
        public DateTime DayDate { get; set; }

        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "0.00")]
        public decimal Price { get; set; }
        public IEnumerable<CompanyMenuItemModel> Items { get; set; }
    }
    public class CompanyMenuItemModel
        {
        public DateTime DayDate { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public int Quantity { get; set; }
        [DisplayFormat(DataFormatString = "0.00")]
        public decimal Price { get; set; }
        [DisplayFormat(DataFormatString ="0.00") ]
        public decimal Amount { get; set; }
    }
}