using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CateringPro.Models
{
    public class UserWeekBasket: UserData
    {

        public DateTime BasketDate { get; set; }
    }
}
