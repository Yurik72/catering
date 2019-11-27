using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CateringPro.Models
{
    public class Ingredients: CompanyDataOwnId
    {
        public Ingredients()
        {
           
            Consignments=new HashSet<Consignment>();
        }

       // public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Required]
        [DisplayName("Ingredient Name")]
        public string Name { get; set; }


        [DisplayName("Ingredients_MeasureUnit")]

        public string MeasureUnit { get; set; }
        [DisplayName("StockValue")]
        public decimal  StockValue { get; set; }
        [DisplayName("StockDate")]
        public DateTime StockDate { get; set; }

        [DisplayName("AvgPrice")]
        public decimal AvgPrice { get; set; }
        public virtual ICollection<Consignment> Consignments { get; set; }
        [DisplayName("Ingredient Category")]
        public int IngredientCategoriesId { get; set; }

        [DisplayName("Ingredient Category")]
        public virtual IngredientCategories IngredientCategory { get; set; }


    }
}