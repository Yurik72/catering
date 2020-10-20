using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public interface ISupportDeactivate
    {
        bool IsDeactivated { get; set; }
    }
}
