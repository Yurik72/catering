﻿namespace CateringPro.Test.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using CateringPro.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;
    using CateringPro.ViewModels;
    public class DiscountControllerTest : GenericControllerTest<Discount, DiscountController>
    {
        protected override void OnEntitiesAdd(DbContext cont, List<Discount> ent)
        {
            base.OnEntitiesAdd(cont, ent);
            ent.ForEach(e => e.Name = "name" + e.Id.ToString());
        }
    }
}
