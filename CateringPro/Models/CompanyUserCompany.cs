using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class CompanyUserCompany
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [StringLength(100)]
        public string CompanyUserId { get; set; }
        public CompanyUser CompanyUser { get; set; }

        [DisplayName("User SubGroup")]
        public int? UserSubGroupId { get; set; }

        [DisplayName("User Group")]
        public int? UserGroupId { get; set; }
    }
}
