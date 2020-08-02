using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class RegisterViewModel
    {
        [Required]

        [DisplayName("UserName")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("NameSurname")]
        public string NameSurname { get; set; }

        [DisplayName("Country")]
        public string Country { get; set; }
        [DisplayName("City")]
        public string City { get; set; }
        [DisplayName("ZipCode")]
        public string ZipCode { get; set; }
        [DisplayName("Address1")]
        public string Address1 { get; set; }
        [DisplayName("Address2")]
        public string Address2 { get; set; }

        [DisplayName("ConfirmedByAdmin")]
        public bool ConfirmedByAdmin { get; set; }


        public List<string> Errors { get; set; }

    }
}
