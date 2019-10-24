using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CateringPro.Models
{
    public class Company
    {
        public int Id { get; set; }

        [StringLength(10)]
    
        public string Code { get; set; }
        [StringLength(40)]
      
        public string Name { get; set; }
        [DisplayName("Phone")]
        [StringLength(40)]
        public string Phone { get; set; }

        [StringLength(40)]
        public string Email { get; set; }

        [DisplayName("ZipCode")]
        [StringLength(10)]
        public string ZipCode { get; set; }
        [DisplayName("Country")]
        [StringLength(15)]
        public string Country { get; set; }
        [DisplayName("City")]
        [StringLength(25)]
        public string City { get; set; }
        [DisplayName("Address1")]
        [StringLength(40)]
        public string Address1 { get; set; }
        [DisplayName("Address2")]
        [StringLength(40)]
        public string Address2 { get; set; }
        [DisplayName("Logo")]
        public int? PictureId { get; set; }

        [DisplayName("StampLogo")]
        public int? StampPictureId { get; set; }
        [DisplayName("LeadTime")]
        public int? OrderLeadTimeH { get; set; }
        [DisplayName("ThresholdTime")]
        public int? OrderThresholdTimeH { get; set; }
        
    }
}
