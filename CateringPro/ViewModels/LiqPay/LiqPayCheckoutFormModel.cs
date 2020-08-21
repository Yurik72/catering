using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CateringPro.ViewModels.LiqPay
{
    /// <summary>
    /// Данні, які передаються у в'ю для формування кнопки оплати LiqPay
    /// </summary>
    public class LiqPayCheckoutFormModel
    {
        public string Data { get; set; }
        public string Signature { get; set; }
    }
}