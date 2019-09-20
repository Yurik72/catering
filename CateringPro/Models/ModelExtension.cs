using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using  System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CateringPro.Models
{
    public abstract class UserData:CompanyData
    {

        public string UserId { get; set; }

        public CompanyUser User { get; set; }


    }
    public abstract class CompanyDataOwnId: CompanyData
    {
        public int Id { get; set; }
    }
    public abstract class CompanyData
    {
        
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }
    }
    public static class ModelExtension
    {
        public static IQueryable<TEntity> WhereU<TEntity>(this IQueryable<TEntity> source) where TEntity : UserData
        {
            return  source.Where(u=>u.UserId =="");  //to do
        }
    }
}
