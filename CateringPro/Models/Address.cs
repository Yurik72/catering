using CateringPro.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class Address : CompanyDataOwnId
    {

     

        public Address()
        {
            Docs = new HashSet<Docs>();
            Consignments= new HashSet<Consignment>();

        }
        [StringLength(50)]
        [DisplayName("Address Code")]
        public string Code { get; set; }
        [StringLength(256)]
        [DisplayName("Name")]
        [DefaultName]
        public string Name { get; set; }
        public int AddressType { get; set; }
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [StringLength(256)]
        [DisplayName("Email")]
        public string Email { get; set; }
        [StringLength(15)]
        [DisplayName("PhoneNumber")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string PhoneNumber { get; set; }

        [DataType(DataType.PostalCode)]
        [StringLength(10)]
        [DisplayName("ZipCode")]
        public string ZipCode { get; set; }

        [StringLength(15)]
        [DisplayName("Country")]
        public string Country { get; set; }
        [StringLength(25)]
        [DisplayName("City")]
        public string City { get; set; }

        [StringLength(40)]
        [DisplayName("Address1")]
        public string Address1 { get; set; }

        [StringLength(40)]
        [DisplayName("Address2")]
        public string Address2 { get; set; }

        public virtual ICollection<Docs> Docs { get; set; }

        public virtual ICollection<Consignment> Consignments { get; set; }
    }
}
