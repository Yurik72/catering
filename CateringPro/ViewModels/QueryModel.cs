using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class QueryModel
    {
        public string SortField { get; set; }
        public string SortOrder { get; set; }
        public int Page { get; set; }
        public string SearchCriteria { get; set; }

        public int RelationFilter { get; set; }

        public int PageRecords { get; set; }
    }
}
