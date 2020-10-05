using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.ViewModels
{
    public class FileViewModel
    {
        public string Name { get; set; }
        public DateTime DateLastModif { get; set; }

        public long Size { get; set; }

        public string URLLink { get; set; }
    }
}
