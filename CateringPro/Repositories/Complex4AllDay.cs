using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface IComplex4AllDayConfig
    {
        int ThresholdComplexNumber { get; }

    }

    public class Complex4AllDayConfig : IComplex4AllDayConfig
    {
        public int ThresholdComplexNumber { get; set; }

    }
    public class Complex4AllDay: IDiscountPlugin
    {
        private IComplex4AllDayConfig config;
        public Complex4AllDay()
        {
           // config = Configuration.GetSection("Complex4AllDay").Get<Complex4AllDayConfig>();
        }
    }
}
