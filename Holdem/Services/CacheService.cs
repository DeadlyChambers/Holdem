using System;
using System.Web;
using CommonCardLibrary;

namespace Holdem.Services
{
    public interface ICacheService
    {
        void Save(Table table);
        Table Get(Guid? id);
    }

    public class CacheService : ICacheService
    {
        public void Save(Table table)
        {
            HttpContext.Current.Cache["currentTable"] = table;
        }

        public Table Get(Guid? id)
        {
            return (Table)HttpContext.Current.Cache["currentTable"];
        }
    }
}
