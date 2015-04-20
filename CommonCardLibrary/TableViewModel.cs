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
        public List<Player> Players{get;set;}
        public Deck Deck { get; set; }
        public int Pot { get; set; }
        public List<Card> Cards { get; set; }
        public bool DealOver { get; set; }
        
        public TableViewModel(Deck deck, List<Player> players) 
        {
            Id = Guid.NewGuid();
            Deck = deck;
            Players = players;
            Cards = new List<Card>();
        }

        public TableViewModel(Deck deck, List<Player> players, List<Card> cards)
        {
            Id = Guid.NewGuid();
            Deck = deck;
            Players = players;
            Cards = cards;
        }

        public void Deal()
        {
            var index = -1;
            foreach (Player player in Players.Where(x => x.Playing))
                player.Cards.Add(Deck.Cards[++index]);
            foreach (Player player in Players.Where(x => x.Playing))
                player.Cards.Add(Deck.Cards[++index]);

        }

        public void BurnAndTurn()
        {
            var count = (Players.Count(x => x.Playing) * 2);
            if(Cards.Count == 0)
            {
                Cards.Add(Deck.Cards[count]);
                Cards.Add(Deck.Cards[count + 1]);
                Cards.Add(Deck.Cards[count + 2]);                
            }
            else if(Cards.Count == 3 || Cards.Count == 4)
            {
                Cards.Add(Deck.Cards[count + Cards.Count+ 1]);
            }
            if(Cards.Count == 5)
                DealOver = true;
            
        }
    }
}
