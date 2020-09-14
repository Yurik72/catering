using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class UserSubGroup:CompanyDataOwnId
    {
        public UserSubGroup()
        {
            UserSubGroups = new HashSet<UserSubGroup>();
            CompanyUsers = new HashSet<CompanyUser>();

        }

        [StringLength(100)]
        [DisplayName("User SubGroup")]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        public virtual UserSubGroup Parent { get; set; }

        public virtual ICollection<UserSubGroup> UserSubGroups { get; set; }

        public virtual ICollection<CompanyUser> CompanyUsers { get; set; }
    }
}
