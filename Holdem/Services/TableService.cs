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
            db.Tables.AddOrUpdate(table);
            db.SaveChanges();
        }

        public TableViewModel Get(Guid? id, int? players)
        {
            return ToTableViewModel(id.HasValue ? db.Tables.Find(id) : db.Tables.FirstOrDefault());
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
            }).ToList();
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