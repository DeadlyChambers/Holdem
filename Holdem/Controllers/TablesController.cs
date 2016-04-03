using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CommonCardLibrary;
using CommonCardLibrary.Entities;
using Glimpse.Mvc.Message;
using Holdem.Models;
using Holdem.Services;
using Holdem.Utilities;
using Newtonsoft.Json;
using WebGrease.Css.Extensions;

namespace Holdem.Controllers
{
    [Authorize]
    public class TablesController : Controller
    {
        private GameContext db = new GameContext();

        private readonly ITableService _service;
        public TablesController(ITableService service)
        {
            _service = service;
        }
        // GET: Tables
        public ActionResult Index()
        {
            return View(db.Tables.ToList());
        }

        // GET: Tables/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound("No table exists for the id");
            }
            var round = db.Rounds.FirstOrDefault(x => x.TableId == table.Id && (!x.Started || x.Active));
            if (round == null)
            {
                return HttpNotFound("No rounds exist, try deleting the table");
            }
            var vm = SetupTableRoundViewModel(round, table);
            return View(vm);
        }

        private TableRoundViewModel SetupTableRoundViewModel(Round round, Table table)
        {
            var players = db.PlayerHands.Include(x => x.Player).Where(x => x.RoundId == round.Id).ToList().AsSafeReadOnly();
            var takenIds = players.Select(x => x.PlayerId);
            var availablePlayers = db.Players.Where(x => x.Cash > table.BuyIn && !takenIds.Contains(x.Id)).ToList().AsSafeReadOnly();
            var vm = new TableRoundViewModel
            {
                Table = table,
                Round = round,
                Players = players,
                AvailablePlayers = availablePlayers
            };
            return vm;
        }

        [HttpPost]
        public ActionResult AddPlayerToTable(AddPlayerViewModel addPlayer)
        {
            string httpNotFound = null;
            var vm = AddSelfToTable(addPlayer, out httpNotFound);
            if (vm == null && httpNotFound != null)
                return HttpNotFound(httpNotFound);
            return PartialView("GamePlayers", vm);
        }

        private TableRoundViewModel AddSelfToTable(AddPlayerViewModel addPlayer,
            out string httpNotFound, bool skipExtra = false)
        {
            var table = db.Tables.Find(addPlayer.TableId);
            if (table == null)
            {
                httpNotFound = "No table exists for the id";
                return null;
            }
            var round =
                 db.Rounds.Include(x => x.Players)
                     .FirstOrDefault(x => x.Id == addPlayer.RoundId && x.Players.Count < Constants.MAX_PLAYERS);
            if (round == null || round.Id == Guid.Empty)
            {
                httpNotFound = "Either the round doesn't exist, or the table is already full. Try another table.";
                return null;
            }
            var player = db.Players.Find(addPlayer.PlayerId);
            if (player == null)
            {
                httpNotFound = "Player does not exist";
                return null;
            }
            if (db.PlayerHands.Find(addPlayer.PlayerId, addPlayer.RoundId) != null)
            {
                httpNotFound = "Player is already a part of the game";
                return null;
            }
            if (player.Cash < table.BuyIn)
            {
                httpNotFound = "Sorry you can't afford the game, go reup";
                return null;
            }
            player.Cash -= table.BuyIn;
            db.PlayerHands.Add(new PlayerHand
            {
                PlayerId = player.Id,
                RoundId = round.Id,
                TotalCash = table.BuyIn,
                Acting = false,
                Active = true,
                Won = false,
                Waiting = skipExtra
            });
            db.SaveChanges();
            httpNotFound = null;
            return skipExtra ? null : SetupTableRoundViewModel(round, table);
        }

        [HttpGet]
        public ActionResult Current(Guid tableId, Guid roundId)
        {

            return Current(new SubmitCardScreenViewModel { TableId = tableId, RoundId = roundId, PlayerId = Guid.Empty, Command = "Null", RaiseAmount = (decimal)0.00 });
        }

        [HttpPost]
        public ActionResult Current(SubmitCardScreenViewModel submission)
        {

            if (submission.Command == "NewEntry") //New person coming to the table need to check in and
                                                  //get put in the table for all of the other things
            {
                string httpNotFound = null;
                var vm = AddSelfToTable(submission, out httpNotFound, true);
                if (httpNotFound != null)
                {
                    ViewBag.NotificationMessage = httpNotFound;
                }
            }

            var table = db.Tables.Find(submission.TableId);
            if (table == null)
                return HttpNotFound("No table exists for the id");
            var round = db.Rounds.Find(submission.RoundId);
            if (round == null)
                return HttpNotFound("The round doesn't exist");
            var playerHands =
                db.PlayerHands.Include(x => x.Player)
                    .Where(x => x.RoundId == submission.RoundId).ToList();

            var playerVms = new List<PlayerViewModel>();
            foreach (var player in playerHands)
                playerVms.Add(new PlayerViewModel(player, player.Player.Name));

            if (playerHands.Count < 3)
                ViewBag.NotificationMessage = "We can't start until we have at least three people";
            var tableVm = new TableViewModel(playerVms, round) { MinBet = table.MinBet };
          

            
            if (!round.Started|| string.IsNullOrEmpty(round.Cards))
            {
                tableVm.CurrentBet = table.MinBet;
                byte count = 1;
                foreach (var player in tableVm.Players)
                    player.Position = count++;
                round.CurrentBet = tableVm.CurrentBet;
                round.Active = true;
                round.Started = true;
                round.Cards = JsonConvert.SerializeObject(tableVm.Cards);
                round.Place = TableRound.PreFlop;
                round.Dealer = tableVm.Dealer;
                InitializePlayers(round.Id, tableVm);
            }
            else if (submission.PlayerId != Guid.Empty && submission.Command != "Null")
            {
                //premptively bringing people to the page that belong
                if (!string.IsNullOrEmpty(ViewBag.NotificationMessage))
                    return View("Current", tableVm);
                var tempPlace = round.Place == TableRound.PreFlop ? (byte)round.Place + 1 : (byte)round.Place * 2;
                tableVm.Place = (TableRound)tempPlace;
                round.Place = tableVm.Place;
                if (submission.Command == "Raise")
                {
                    tableVm.CurrentBet += submission.RaiseAmount;
                    round.CurrentBet = tableVm.CurrentBet;
                }
                tableVm.DetermineWinner();
                if (round.Place == TableRound.End)
                {
                    var nextRound = Guid.NewGuid();
                    db.Rounds.Add(new Round
                    {
                        TableId = table.Id,
                        Dealer = tableVm.Dealer,
                        Id = nextRound,
                        Active = true
                    });
                    foreach (var player in tableVm.Players)
                    {
                        db.PlayerHands.Add(new PlayerHand
                        {
                            TotalCash = player.TotalCash,
                            PlayerId = player.Id,
                            RoundId = nextRound
                        });
                    }
                    db.SaveChanges();
                    return
                        Current(new SubmitCardScreenViewModel()
                        {
                            TableId = submission.TableId,
                            RoundId = nextRound,
                            PlayerId = Guid.Empty,
                            Command = "Null",
                            RaiseAmount = (decimal)0.00
                        });
                }

              
            }
            db.SaveChanges();
            return View(tableVm);
        }

        private void InitializePlayers(Guid roundId, TableViewModel tableVm)
        {
            //See if we have an acting player if we do, let's move to the next
            //player. Ensuring that we don't go over the limit we have of players
            var currentActing = tableVm.Players.Find(x => x.Acting)?.Position;
            if (currentActing == null || currentActing >= tableVm.Players.Count)
                currentActing = 1;
            currentActing++;
            tableVm.DetermineWinner();
            foreach (var player in tableVm.Players)
            {
                player.Waiting = false;
                var playerEntity = db.PlayerHands.Find(player.Id, roundId);
                playerEntity.Cards = JsonConvert.SerializeObject(player.Cards);
                playerEntity.Position = player.Position;
                playerEntity.Won = player.Won;
                playerEntity.Acting = false;
                playerEntity.Waiting = false;
                player.Acting = false;
                if (player.Position == currentActing)
                {
                    playerEntity.Acting = true;
                    player.Acting = true;
                }
            }
        }


        // GET: Tables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,MinBet,MaxBet,BuyIn")] Table table)
        {
            if (ModelState.IsValid)
            {
                table.Id = Guid.NewGuid();
                table.Active = true;
                db.Tables.Add(table);
                db.Rounds.Add(new Round
                {
                    TableId = table.Id,
                    Dealer = 1,
                    Id = Guid.NewGuid(),
                    Active = true
                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(table);
        }

        // GET: Tables/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Tables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,MinBet,MaxBet,BuyIn,Active")] Table table)
        {
            if (ModelState.IsValid)
            {
                db.Entry(table).State = EntityState.Modified;
                var round = db.Rounds.First(x => x.TableId == table.Id && !x.Started && !x.Active);
                round.Active = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(table);
        }

        // GET: Tables/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Table table = db.Tables.Find(id);
            table.Active = false;
            //db.Tables.Remove(table);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
