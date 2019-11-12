using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using System.Linq.Expressions;
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
        public static CustomTypeSqlQuery<T> SqlQuery<T>(
        this DatabaseFacade database,
        string sqlQuery) where T : class, new()
        {
            return new CustomTypeSqlQuery<T>()
            {
                DatabaseFacade = database,
                SQLQuery = sqlQuery
            };
        }
    }
    public class CustomTypeSqlQuery<T> where T : class,new()
    {
        private Action<IDataRecord, T> action_materilaze;
        private AutoMaterilize<T> auto;
        public Action<IDataRecord, T> Action_materilaze
        {
            get
            {
               if( action_materilaze!=null)
                    return this.action_materilaze;
                if (auto == null)
                    auto = new AutoMaterilize<T>();
                this.action_materilaze = auto.Materialize;
                return this.action_materilaze;
            }
             
        }

        public DatabaseFacade DatabaseFacade { get; set; }
        public string SQLQuery { get; set; }

        public CustomTypeSqlQuery()
        {
            
        }
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
                            Action_materilaze(reader, x);
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
    public class AutoMaterilize<T> where T : class, new()
    {
        bool isBuild = false;
        Dictionary<int, Action<object, object>> fieldset = new Dictionary<int, Action<object, object>>();
        public void Materialize(IDataRecord record,T src)
        {
            if (!isBuild)
                Build(record,src);
            foreach (var it in fieldset)
                it.Value(src, record.GetValue(it.Key));

        }
        private void Build(IDataRecord record, T src)
        {
            for(int i = 0; i < record.FieldCount; i++)
            {
                var action = typeof(T).PropertySet(record.GetName(i));
                if (action != null)
                    fieldset.Add(i, action);
            }
            isBuild = true;
        }
    }
}
