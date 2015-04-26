using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CommonCardLibrary;

namespace Holdem.Services
{
    public static class RandomExtension
    {
        public static void Shuffle<T>(this Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }

    public class GameContextInitializer 
        //: DropCreateDatabaseAlways<GameContext>
    {
        

        public static TableViewModel GetTableViewModel(int? playerTotal)
        {
            var names = new string[]
            {
                "John Query", "Jon Snow", "Taylor Swift", "Elvis", "Dave Chapelle",
                "John Stamos", "Ray Charles", "John Cena"
            };
            if(!playerTotal.HasValue)
                new Random().Shuffle(names);

            var deckVm = new List<Card>();
            deckVm.InitializeDeck();
            var playersVm = new List<PlayerViewModel>();
            for (var x = 0; x < playerTotal; x++)
                playersVm.Add(new PlayerViewModel(names[x], x));
            var t = new TableViewModel(deckVm, playersVm);
            return t;
        }

        //protected override void Seed(GameContext context)
        //{

        //    var t = GetTableViewModel(8);

        //    var players = t.Players.Select(x => new Player
        //    {
        //        Id = x.Id,
        //        Name = x.Name,
        //        Playing = x.Playing,
        //        Position = x.Position,
        //        CardsJson = x.Cards.ToJson(),
        //        Chips = x.Chips,
        //        HandEnum = (int)x.Hand,
        //        HandStrength = x.HandStrength,
        //        WinningHand = x.WinningHand
        //    }).ToList();
        //    context.Players.AddRange(players);
        //    context.SaveChanges();
        //    var table = new Table
        //    {
        //        Id = t.Id,
        //        Players = players,
        //        DeckJson = t.Deck.ToJson(),
        //        Pot = t.Pot,
        //        CardsJson = t.Cards.ToJson(),
        //        DealOver = t.DealOver
        //    };
        //    context.Tables.Add(table);
        //    context.SaveChanges();

        //}
    }
}