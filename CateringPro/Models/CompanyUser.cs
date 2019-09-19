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

    }
    public class CompanyRole : IdentityRole
    {

       // public int CompanyId { get; set; }

    }
}
