﻿using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface IPluginsRepository
    {
        IDiscountPlugin GetDiscointPlugin();
    }
}
