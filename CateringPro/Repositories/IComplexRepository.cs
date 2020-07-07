﻿using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface IComplexRepository
    {

        Task<bool> UpdateComplexDishes(Complex complex, List<string> dishes, int companyid, List<ItemsLine> dishLine);

    }
}
