using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class UserFinIncome:CompanyData
    {
        [StringLength(100)]
        public string Id { get; set; }
        [DefaultValue(0)]
        public int IncomeType { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [DefaultValue(0.0)]
        public decimal Amount { get; set; }
        [StringLength(200)]
        [DefaultValue("")]
        public string TransactionData { get; set; }

        public DateTime TransactionDate { get; set; }
        [StringLength(200)]
        [DefaultValue("")]
        public string Comments { get; set; }

        public UserFinance UserFinance { get; set; }
    }
}
