using Microsoft.Extensions.Configuration;
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
        decimal DiscountValue { get; }

    }

    public class Complex4AllDayConfig : IComplex4AllDayConfig
    {
        public int ThresholdComplexNumber { get; set; }
        public decimal DiscountValue { get; set; }
    }
    public class Complex4AllDay: IDiscountPlugin
    {
        private  IComplex4AllDayConfig myconfig;
        public Complex4AllDay()
        {
           // config = Configuration.GetSection("Complex4AllDay").Get<Complex4AllDayConfig>();
        }
        public void LoadConfig(IConfiguration config)
        {
            myconfig= config.GetSection("Complex4AllDay").Get<Complex4AllDayConfig>();
        }
    }
}
