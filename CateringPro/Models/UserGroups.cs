using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CateringPro.Models
{
    public class UserGroups: CompanyDataOwnId
    {
        public UserGroups()
        {
             CompanyUsers =new  HashSet<CompanyUser>();
        }


        [StringLength(100, MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Required]
        [DisplayName("MenuUserGroups")]
        public string Name { get; set; }


        [DisplayName("User Group")]
        public virtual UserGroups UserGroup { get; set; }

        public virtual ICollection<CompanyUser> CompanyUsers { get; set; }
    }
}