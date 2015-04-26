using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCardLibrary
{
    public partial class TableViewModel
    {
        public Guid Id { get; set; }
        public List<PlayerViewModel> Players{get;set;}
        public List<Card> Deck { get; set; }
        public int Pot { get; set; }
        public List<Card> Cards { get; set; }
        public bool DealOver { get; set; }

        public TableViewModel()
        {
            Id = Guid.NewGuid();
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
            foreach (PlayerViewModel player in Players.Where(x => x.Playing))
                player.Cards.Add(Deck[++index]);
            foreach (PlayerViewModel player in Players.Where(x => x.Playing))
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

        public void BurnAndTurn()
        {
            var count = (Players.Count(x => x.Playing) * 2);
            if(Cards.Count == 0)
            {
                Cards.Add(Deck[count]);
                Cards.Add(Deck[count + 1]);
                Cards.Add(Deck[count + 2]);                
            }
            else if(Cards.Count == 3 || Cards.Count == 4)
            {
                Cards.Add(Deck[count + Cards.Count+ 1]);
            }
            if(Cards.Count == 5)
                DealOver = true;
            
        }
    }
}
