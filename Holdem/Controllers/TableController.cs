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
        private readonly ITableService _service;
        public TableController(ITableService service)
        {
            _service = service;
        }
        // GET: Table
        public ActionResult Current(int? playerCount)
        {
            var deck = new Deck();
            var players = new List<Player>();
            playerCount = playerCount ?? 1;
            for (var x = 0; x < playerCount; x++)
                players.Add(new Player());
            var table = new TableViewModel(deck, players);
            table.Deal();
            _service.Save(table);
            return View("Current", table);
        } 

        [HttpPost]
        public ActionResult Current(string Command, Guid? tableId)
        {
            var currentTable = _service.Get(tableId);
            if (!currentTable.DealOver && (Command == "Bet" || Command == "Check"))
            {
                currentTable.BurnAndTurn();
                var leader =currentTable.DetermineWinner();
                
                foreach (var player in currentTable.Players)
                {
                    player.WinningHand = leader.Count(x => x.Id == player.Id) == 1;
                }
                
                _service.Save(currentTable);
            }
            else
                return RedirectToAction("Current",new {playerCount = currentTable.Players.Count(x => x.Playing)}
        );
            return View("Current", currentTable);
        }
    }

  
}