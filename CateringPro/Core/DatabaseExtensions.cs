using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Core
{
    public static class DatabaseExtensions
    {
        public static CustomTypeSqlQuery<T> SqlQuery<T>(
                this DatabaseFacade database,
                string sqlQuery,
                Action<IDataRecord, T> materailize) where T : class,new()
        {
            return new CustomTypeSqlQuery<T>(materailize)
            {
                DatabaseFacade = database,
                SQLQuery = sqlQuery
            };
        }
    }
    public class CustomTypeSqlQuery<T> where T : class,new()
    {
        Action<IDataRecord, T> action_materilaze;

        public DatabaseFacade DatabaseFacade { get; set; }
        public string SQLQuery { get; set; }

        public CustomTypeSqlQuery(Action<IDataRecord,T> materailize)
        {
            action_materilaze = materailize;
        }

        public async Task<IList<T>> ToListAsync()
        {
            IList<T> results = new List<T>();
            var conn = DatabaseFacade.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = SQLQuery;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            T x = new T();
                            action_materilaze(reader, x);
                            results.Add(x);
                        }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }
            return results;
        }

       
    }
}
