using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class ErrorViewModel
    {
        

        [ScaffoldColumn(false)]
        public string ReturnUrl { get; set; }

        public bool IsModal { get; set; }

        public string Description { get; set; }
    }
}
