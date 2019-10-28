using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class DocLines : CompanyDataOwnId
    {

        public int DocsId { get; set; }

        public int Number { get; set; }

        public int IngredientsId { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }

        public virtual Docs Docs { get; set; }

        public virtual Ingredients Ingredients { get; set; }


    }
}
