using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CateringPro.Models
{
    public class Company
    {
        public int Id { get; set; }

        [StringLength(10)]
        [Required]
        public string Code { get; set; }
        [StringLength(40)]
        [Required]
        public string Name { get; set; }
    }
}
