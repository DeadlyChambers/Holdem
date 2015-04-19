using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCardLibrary
{
    public class Player
    {
        public Guid Id { get; set; }
        public bool Playing { get; set; }
        public int Position { get; set; }
        public List<Card> Cards { get; set; }
        public int Chips { get; set; }
        public Hand Hand { get; set; }
        public int HandStrength { get; set; }
        public bool WinningHand { get; set; }
        public Player()
        {
            Id = Guid.NewGuid();
            Playing = true;
            Hand = Hand.HighCard;
            Cards = new List<Card>();
        }
    }

    
}
