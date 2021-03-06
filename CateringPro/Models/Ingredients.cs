﻿using CateringPro.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "decimal(18,2)")]
        public decimal  StockValue { get; set; }
        [DisplayName("StockDate")]
        public DateTime StockDate { get; set; }

        [DisplayName("AvgPrice")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AvgPrice { get; set; }
        [DisplayName("LastPurchasePrice")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal LastPurchasePrice { get; set; }
        public virtual ICollection<Consignment> Consignments { get; set; }
        [DisplayName("Ingredient Category")]
        public int IngredientCategoriesId { get; set; }

        [DisplayName("Ingredient Category")]
        [DefaultInclude]
        public virtual IngredientCategories IngredientCategory { get; set; }


    }
}