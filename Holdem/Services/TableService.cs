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
            var table = ToTable(tableViewModel);
            var players = table.Players;
            //table.GameId = GameStuff.GameId;
            db.Tables.AddOrUpdate(table);
            foreach (var player in players)
            {
                var tPlayer = player;
                  //  tPlayer.GameId = GameStuff.GameId;
                db.Players.AddOrUpdate(tPlayer);
                
            }
            var game = new Game()
            {
                Id = GameStuff.GameId,
                Players = players,
                Tables = new List<Table>() { table}
            };
            //db.Games.AddOrUpdate(game);
            db.SaveChanges();
        }

        public TableViewModel Get(Guid? id, int? players)
        {
            var table = db.Tables.Find(id);
            if (table == null)
            {
               var tm = GameContextInitializer.GetTableViewModel(players.Value);
                Save(tm);
                return tm;
            }
            return ToTableViewModel(table);
        }

        private List<PlayerViewModel> ToPlayerViewModel(List<Player> players)
        {
            return players.Select(x => new PlayerViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Playing = x.Playing,
                Position = x.Position,
                Cards = x.CardsJson.ToCards(),
                Chips = x.Chips,
                Hand = EnumParse.HandDictionary[x.HandEnum.ToString()],
                HandStrength = x.HandStrength,
                WinningHand = x.WinningHand
            }).OrderBy(y => y.Position).ToList();
        } 

        private List<Player> ToPlayers(List<PlayerViewModel> playersVm)
        {
            return playersVm.Select(x => new Player
            {
                Id = x.Id,
                Name = x.Name,
                Playing = x.Playing,
                Position = x.Position,
                CardsJson = x.Cards.ToJson(),
                Chips = x.Chips,
                HandEnum = (int)x.Hand,
                HandStrength = x.HandStrength,
                WinningHand = x.WinningHand
            }).ToList();
        }

        private TableViewModel ToTableViewModel(Table t)
        {
            return new TableViewModel
            {
                Id = t.Id,
                Players = ToPlayerViewModel(t.Players),
                Deck = t.DeckJson.ToCards(),
                Pot = t.Pot,
                Cards = t.CardsJson.ToCards(),
                DealOver = t.DealOver
            };
        }

        private Table ToTable(TableViewModel t)
        {
            return new Table
            {
                Id = t.Id,
                Players = ToPlayers(t.Players),
                DeckJson = t.Deck.ToJson(),
                Pot = t.Pot,
                CardsJson = t.Cards.ToJson(),
                DealOver = t.DealOver
            };
        }
    }
}