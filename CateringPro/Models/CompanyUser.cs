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

        public int?  MenuType { get; set; }
    }
    public class CompanyRole : IdentityRole
    {

       // public int CompanyId { get; set; }

    }
}
