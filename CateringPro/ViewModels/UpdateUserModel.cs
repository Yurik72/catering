using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using CateringPro.Models;
namespace CateringPro.ViewModels
{

    public class UpdateUserModel
    {
        public UpdateUserModel() { 
        }
        public UpdateUserModel(CompanyUser usr)
        {
            this.CopyFrom(usr);
        }

        public string Id { get; set; }

        public string UserName { get; set; }
        

        [DisplayName("NewPassword")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [NotMapped] // Does not effect with your database
        [Compare("NewPassword")]
        [DataType(DataType.Password)]
        [DisplayName("ConfirmPassword")]
        public string ConfirmPassword { get; set; }


        [DisplayName("UpdateUserOldPassword")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [DisplayName("UpdateUserEmail")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("EmailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [DataType(DataType.PhoneNumber)]

        [DisplayName("PhoneNumber")]
        public string PhoneNumber { get; set; }


        [DataType(DataType.PostalCode)]
        [DisplayName("ZipCode")]
        public string ZipCode { get; set; }


        [DisplayName("Country")]
        
        public string Country { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("Address1")]
        public string Address1 { get; set; }

        [DisplayName("Address2")]
        public string Address2 { get; set; }

        [DisplayName("NameSurname")]
        public string NameSurname { get; set; }

        [DisplayName("UserGroupId")]
        public int? UserGroupId { get; set; }

        [DisplayName("UserSubGroupId")]
        public int? UserSubGroupId { get; set; }

        //[DisplayName("User Group")]
       // public virtual UserGroups UserGroup { get; set; }

        [DefaultValue(false)]
        public bool ConfirmedByAdmin { get; set; }
        [DisplayName("ChildrenCount")]
        public int ChildrenCount { get; set; }
        [DisplayName("ChildNameSurname")]
        public string ChildNameSurname { get; set; }
        [DisplayName("ChildBirthdayDate")]
        public DateTime? ChildBirthdayDate { get; set; }
        [DisplayName("PictureId")]
        public int? PictureId { get; set; }
        public bool IsNew { get; set; }
        [DisplayName("CompanyName")]
        public string CompanyName { get; set; }
        public bool IsChild { get; set; }
        public bool IsModal { get; set; }
        [ScaffoldColumn(false)]
        public string ReturnUrl { get; set; }
        public List<string> Errors { get; set; }
        public UpdateUserModel CopyFrom(CompanyUser usr)
        {
            if (usr != null)
            {
                this.Id = usr.Id;
                this.Email = usr.Email;
                this.ZipCode = usr.ZipCode;
                this.Address1 = usr.Address1;
                this.Address2 = usr.Address2;
                this.City = usr.City;
                this.Country = usr.Country;
                this.PhoneNumber = usr.PhoneNumber;
                this.NameSurname = usr.NameSurname;
                this.UserName=usr.UserName;
                this.UserGroupId = usr.UserGroupId;
                this.UserSubGroupId = usr.UserSubGroupId;
                this.ConfirmedByAdmin = usr.ConfirmedByAdmin;
                this.ChildrenCount = usr.ChildrenCount;
                this.ChildNameSurname = usr.ChildNameSurname;
                this.ChildBirthdayDate = usr.ChildBirthdayDate;
                this.EmailConfirmed = usr.EmailConfirmed;
                this.PictureId = usr.PictureId;
    }

            return this;
        }
        public CompanyUser CopyEditedModalDataTo(CompanyUser usr, bool isNew = false)
        {
            if (usr != null)
            {
                if (!string.IsNullOrEmpty(this.Id))
                    usr.Id = this.Id;
                usr.Email = this.Email;
                usr.City = this.City;
                usr.PhoneNumber = this.PhoneNumber;
                usr.NameSurname = this.NameSurname;
                if (!string.IsNullOrEmpty(this.UserName))
                    usr.UserName = this.UserName;
                usr.UserGroupId = this.UserGroupId;
                usr.UserSubGroupId = this.UserSubGroupId;
                usr.ConfirmedByAdmin = this.ConfirmedByAdmin;
                if (isNew)
                    usr.Email = this.Email;
            }

            return usr;
        }

        public CompanyUser CopyTo(CompanyUser usr, bool isNew = false)
        {
            if (usr != null)
            {
                if (!string.IsNullOrEmpty(this.Id))
                    usr.Id = this.Id;
                if (!string.IsNullOrEmpty(this.Email))
                    usr.Email = this.Email;
                usr.ZipCode = this.ZipCode;
                usr.Address1 = this.Address1;
                usr.Address2 = this.Address2;
                usr.City = this.City;
                usr.PhoneNumber = this.PhoneNumber;
                usr.NameSurname = this.NameSurname;
                if (!string.IsNullOrEmpty(this.UserName))
                    usr.UserName = this.UserName;
                usr.UserGroupId = this.UserGroupId;
                usr.UserSubGroupId = this.UserSubGroupId;
                usr.ConfirmedByAdmin = this.ConfirmedByAdmin;
                usr.ChildrenCount = this.ChildrenCount;
                usr.ChildNameSurname = this.ChildNameSurname;
                usr.ChildBirthdayDate = this.ChildBirthdayDate;
                // usr.EmailConfirmed = this.EmailConfirmed;
                usr.PictureId = this.PictureId;
                if (isNew)
                    usr.Email = this.Email;
            }

            return usr;
        }

        public CompanyUser CopyEditedParamsTo(CompanyUser usr, bool isNew = false)
        {
            if (usr != null)
            {
                if (!string.IsNullOrEmpty(this.Id))
                    usr.Id = this.Id;
                if(!string.IsNullOrEmpty(this.Email))
                    usr.Email=this.Email;
                usr.City = this.City;
                usr.PhoneNumber = this.PhoneNumber;
                usr.NameSurname = this.NameSurname;
                if (!string.IsNullOrEmpty(this.UserName))
                    usr.UserName = this.UserName;
                usr.ChildNameSurname = this.ChildNameSurname;
                usr.ChildBirthdayDate = this.ChildBirthdayDate;
                if (this.PictureId != null && usr.PictureId == null)
                {
                    usr.PictureId = this.PictureId;
                }
                if (isNew)
                    usr.Email = this.Email;
            }

            return usr;
        }


        public CompanyUser CopyUpdateParamsTo(CompanyUser usr, bool isNew = false)
        {
            if (usr != null)
            {
                if (!string.IsNullOrEmpty(this.Id))
                    usr.Id = this.Id;
                if (!string.IsNullOrEmpty(this.Email))
                    usr.Email = this.Email;
                usr.City = this.City;
                usr.Country = this.Country;
                usr.PhoneNumber = this.PhoneNumber;
                usr.NameSurname = this.NameSurname;
                usr.Address1 = this.Address1;
                usr.Address2 = this.Address2;
                usr.ZipCode = this.ZipCode;
                if (!string.IsNullOrEmpty(this.UserName))
                    usr.UserName = this.UserName;
                usr.ChildNameSurname = this.ChildNameSurname;
                usr.ChildBirthdayDate = this.ChildBirthdayDate;
                if(this.PictureId != null && usr.PictureId == null)
                {
                    usr.PictureId = this.PictureId;
                }
                if (isNew)
                    usr.Email = this.Email;
            }

            return usr;
        }


        public bool IsPasswordChanged {
            get => !string.IsNullOrEmpty(this.NewPassword) && !string.IsNullOrEmpty(this.ConfirmPassword); }

        public void InitializeNew()
        {
            this.IsNew = true;
        }

    }
}
