using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using  System.ComponentModel.DataAnnotations;

namespace CateringPro.Models
{
    public abstract class UserData
    {

        public Guid UserId { get; set; }
    }
    public static class ModelExtension
    {
        public static IQueryable<TEntity> WhereU<TEntity>(this IQueryable<TEntity> source) where TEntity : UserData
        {
            return  source.Where(u=>u.UserId ==Guid.Empty);  //to do
        }
    }
}
