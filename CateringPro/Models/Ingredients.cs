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
            PizzaIngredients = new HashSet<PizzaIngredients>();
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
        public virtual ICollection<PizzaIngredients> PizzaIngredients { get; set; }

        public virtual ICollection<Consignment> Consignments { get; set; }

    }
}