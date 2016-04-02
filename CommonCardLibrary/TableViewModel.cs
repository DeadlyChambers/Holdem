using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonCardLibrary.Entities;
using Newtonsoft.Json;

namespace CommonCardLibrary
{
    public partial class TableViewModel
    {
        public Guid Id { get; set; }
        public Guid RoundId { get; set; }
        public List<PlayerViewModel> Players{get;set;}
        public List<Card> Deck { get; set; }
        public decimal Pot { get; set; }
        public byte Dealer { get; set; }
        public decimal CurrentBet { get; set; }
        public decimal MinBet { get; set; } 
        public TableRound Place { get; set; }

        public Guid PlayerId { get; set; }
        /// <summary>
        /// These are the community cards
        /// </summary>
        public List<Card> Cards { get; set; }
        
        public TableViewModel()
        {
            Id = Guid.NewGuid();
        }

        public TableViewModel(List<PlayerViewModel> players, Round round)
        {
            Players = players;
           
            if (!round.Started)
            {
                var count = (Players.Count(x => x.Active)*2);
                var deckVm = new List<Card>();
                deckVm.InitializeDeck();
                Deck = deckVm;
                Deal();
                Cards = new List<Card> {Deck[count++], Deck[count++], Deck[count++], Deck[++count], Deck[count+2] };
            }
            else
            {
                Cards = JsonConvert.DeserializeObject<List<Card>>(round.Cards);
            }

            RoundId = round.Id;
            Id = round.TableId;
            Place = round.Place;
        }

        public TableViewModel(List<Card> deck, List<PlayerViewModel> players) 
        {
            Id = Guid.NewGuid();
            Deck = deck;
            Players = players;
            Cards = new List<Card>();
           
        }

        public TableViewModel(List<Card> deck, List<PlayerViewModel> players, List<Card> cards)
        {
            Id = Guid.NewGuid();
            Deck = deck;
            Players = players;
            Cards = cards;
        }

        public void Deal()
        {
            var index = -1;
            foreach (PlayerViewModel player in Players.Where(x => x.Active))
                player.Cards.Add(Deck[++index]);
            foreach (PlayerViewModel player in Players.Where(x => x.Active))
                player.Cards.Add(Deck[++index]);

        }

        //Only to be used for test purposes
        public void TurnCardSpecific(params Card[] cards)
        {
            foreach (var card in cards)
            {
                Cards.Add(card);
            }
        }
    }
}
