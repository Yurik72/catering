﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CateringPro.Core;
using CateringPro.Data;
using Microsoft.EntityFrameworkCore;

namespace CateringPro.Repositories
{
    public static class UserExtension
    {
        public static void AssignUserAttr<TEntity>(this Controller ctl, TEntity entity) where TEntity : UserData
        {
            entity.CompanyId = ctl.User.GetCompanyID();
            entity.UserId = ctl.User.GetUserId();
        }
        public static async Task<IActionResult> UpdateUserDataAsync<TEntity>(this Controller ctl, TEntity entity, AppDbContext _context, ILogger<CompanyUser> _logger) where TEntity : UserData
        {
            if (!ctl.ModelState.IsValid)
                return ctl.PartialView(entity);
            bool res = await ctl.UpdateDBUserDataAsync(entity, _context, _logger);
            if (!res)
                return ctl.NotFound();
            return ctl.Json(new { res = "OK" });

        }
        public static async Task<bool> UpdateDBUserDataAsync<TEntity>(this Controller ctl, TEntity entity, AppDbContext _context, ILogger<CompanyUser> _logger) where TEntity : UserData
        {
            try
            {
                ctl.AssignUserAttr(entity);
                _context.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exdb)
            {
                _logger.LogError(exdb, "Update {0}", entity.GetType().Name);
              //  if (_context.Find(entity.GetType(), entity.Id) == null)
              //  {
              //      return false;
              //  }
                return false;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update {0}", entity.GetType().Name);
                return false; //to do
            }
            return true;
        }
    }
}
