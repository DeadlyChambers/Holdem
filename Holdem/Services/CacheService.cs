using System;
using System.Web;
using CommonCardLibrary;

namespace Holdem.Services
{
    public interface ITableService
    {
        void Save(TableViewModel tableViewModel);
        TableViewModel Get(Guid? id, int? players);
    }

    public class CacheService : ITableService
    {
        public void Save(TableViewModel tableViewModel)
        {
            HttpContext.Current.Cache["currentTable"] = tableViewModel;
        }

        public TableViewModel Get(Guid? id, int? players)
        {
            if (id == null)
                return GameContextInitializer.GetTableViewModel(players??4);
            return (TableViewModel)HttpContext.Current.Cache["currentTable"];
        }
    }
}
