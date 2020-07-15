using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class UserRoleViewModel
    {
        public string userId { get; set; }
        public string RoleName { get; set; }

        public bool IsAssigned { get; set; }
    }
}
