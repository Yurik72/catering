using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class AssignedCompanyEditViewModel
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public bool IsAssigned { get; set; }
    }
}
