using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CateringPro.Models
{
    public class CompanyUser :IdentityUser<string>
    {
        public CompanyUser()
        {
            this.CompanyUserCompany = new HashSet<CompanyUserCompany>();
        }
        public int CompanyId { get; set; }

        [StringLength(10)]
        public string ZipCode { get; set; }

        [StringLength(15)]
        public string Country { get; set; }
        [StringLength(25)]
        public string City { get; set; }

        [StringLength(40)]
        public string Address1 { get; set; }

        [StringLength(40)]
        public string Address2 { get; set; }

        [StringLength(40)]
        public string NameSurname { get; set; }


        [DisplayName("User Group")]
        public int UserGrpoupId { get; set; }

        [DisplayName("User Group")]
        public virtual UserGroups UserGroup { get; set; }

        public int?  MenuType { get; set; }

        [DefaultValue(false)]
        public bool ConfirmedByAdmin { get; set; }

        public virtual ICollection<CompanyUserCompany> CompanyUserCompany { get; set; }
    }
    public class CompanyRole : IdentityRole
    {

       // public int CompanyId { get; set; }

    }
}
