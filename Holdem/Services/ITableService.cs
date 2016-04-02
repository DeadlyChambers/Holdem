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
  
}
