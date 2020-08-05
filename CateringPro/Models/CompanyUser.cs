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
        public int? UserGroupId { get; set; }

        [DisplayName("User Group")]
        public virtual UserGroups UserGroup { get; set; }

        public int?  MenuType { get; set; }

        [DefaultValue(false)]
        public bool ConfirmedByAdmin { get; set; }

        public virtual ICollection<CompanyUserCompany> CompanyUserCompany { get; set; }

        [StringLength(100)]
        public string ParentUserId { get; set; }
        [StringLength(40)]
        public string ChildNameSurname { get; set; }

        public DateTime? ChildBirthdayDate { get; set; }

        [DefaultValue(1)]
        public int ChildrenCount { get; set; }

        public UserFinance UserFinance { get; set; }

        public int? PictureId { get; set; }
        public string GetChildUserName()
        {
            return string.IsNullOrEmpty(ChildNameSurname) ?
                (string.IsNullOrEmpty(NameSurname) ? UserName : NameSurname)
                : ChildNameSurname;
        } 
        public bool IsChild()
        {
            return !string.IsNullOrEmpty(this.ParentUserId);
        }
    }
    public class CompanyRole : IdentityRole
    {

       // public int CompanyId { get; set; }

    }
}
