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

    public enum UserTypeEnum : int
    {
        [Display(Name = "Child")]
        Child = 0,
        Adult=1,
        Employee = 2

    }
    public class CompanyUser :IdentityUser<string>, ISupportDeactivate
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

        //[DisplayName("RegisterDate")]
        //public DateTime? RegisterDate { get; set; }

        [DefaultValue(1)]
        public int ChildrenCount { get; set; }

      //  public UserFinance UserFinance { get; set; }

        public int? PictureId { get; set; }

        [DisplayName("User SubGroup")]
        public int? UserSubGroupId { get; set; }

        [DisplayName("User SubGroup")]
        public virtual UserSubGroup UserSubGroup { get; set; }

        [StringLength(64)]
        public string CardTag { get; set; }

        [DisplayName("UserType")]
        public int UserType { get; set; }

        [NotMapped]
        public UserTypeEnum UserTypeEn
        {
            get => (UserTypeEnum)this.UserType;
            set => this.UserType = (int)value;
        }
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

        [NotMapped]
        public bool IsChildType => UserTypeEn == UserTypeEnum.Child;
        [NotMapped]
        public bool IsAddultType => UserTypeEn != UserTypeEnum.Child;

        public bool IsDeactivated { get ; set; }

        public long TelegramId { get; set; }

        [NotMapped]
        public ICollection<CompanyUser> UserChilds;
    }
    public class CompanyRole : IdentityRole
    {

       // public int CompanyId { get; set; }

    }
}
