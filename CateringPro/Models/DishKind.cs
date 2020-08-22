using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CateringPro.Models
{
    public class DishKind: CompanyDataOwnId
    {
        public DishKind()
        {

            Complexes = new HashSet<Complex>();

        }


        [StringLength(10)]
        //[Required]

        [DisplayName("Category Code")]
        public string Code { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [StringLength(255, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        [DisplayName("DishKind Decsciption")]
        public string Description { get; set; }




        public ICollection<Complex> Complexes { get; set; }
    }
}