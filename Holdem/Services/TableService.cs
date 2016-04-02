using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using CommonCardLibrary;

namespace Holdem.Services
{
    public class TableService : ITableService
    {
        private GameContext db = new GameContext();
        public void Save(TableViewModel tableViewModel)
        {
            
        }

        public TableViewModel Get(Guid? id, int? players)
        {
           
            return new TableViewModel();
        }


    }
}