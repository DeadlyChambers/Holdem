using CommonCardLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Holdem.Services;

namespace Holdem.Controllers
{
    

    public class TableController : Controller
    {
        


        // GET: Table
        //public ActionResult Current(int? playerCount)
        //{
        //    var table = _service.Get(null, playerCount);
        //    table.Deal();
        //    DetermineLeader(table);
        //    _service.Save(table);
        //    return View("Current", table);
        //} 

        //[HttpPost]
        //public ActionResult Current(string Command, Guid? id)
        //{
        //    var currentTable = _service.Get(id, null);
        //    if (!currentTable.DealOver && (Command == "Bet" || Command == "Check"))
        //    {
        //        currentTable.BurnAndTurn();
        //        DetermineLeader(currentTable);
        //    }
        //    else
        //        return RedirectToAction("Current",new {playerCount = currentTable.Players.Count(x => x.Playing)}
        //);
        //    return View("Current", currentTable);
        //}

        //private void DetermineLeader(TableViewModel currentTable)
        //{
        //    currentTable.DetermineWinner();
        //    _service.Save(currentTable);
        //}
    }

  
}