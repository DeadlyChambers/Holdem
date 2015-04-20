using System;
using System.Web;
using CommonCardLibrary;

namespace Holdem.Services
{
    public interface ITableService
    {
        void Save(TableViewModel tableViewModel);
        TableViewModel Get(Guid? id);
    }

    public class CacheService : ITableService
    {
        public void Save(TableViewModel tableViewModel)
        {
            HttpContext.Current.Cache["currentTable"] = tableViewModel;
        }

        public TableViewModel Get(Guid? id)
        {
            return (TableViewModel)HttpContext.Current.Cache["currentTable"];
        }
    }

    public class TableService : ITableService
    {
        public void Save(TableViewModel tableViewModel)
        {
            HttpContext.Current.Cache["currentTable"] = tableViewModel;
        }

        public TableViewModel Get(Guid? id)
        {
            return (TableViewModel)HttpContext.Current.Cache["currentTable"];
        }
    }


}
