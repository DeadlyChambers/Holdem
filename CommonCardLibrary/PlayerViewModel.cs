using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCardLibrary
{
    public class PlayerViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Playing { get; set; }
        public int Position { get; set; }
        public List<Card> Cards { get; set; }
        public int Chips { get; set; }
        public Hand Hand { get; set; }
        public int HandStrength { get; set; }
        public bool WinningHand { get; set; }
        public PlayerViewModel(string name, int position) : base()
        {
            Name = name;
            Position = position;
            InitializePlayer();
        }

        public PlayerViewModel()
        {
           InitializePlayer();
        }

        private void InitializePlayer()
        {
            Id = Guid.NewGuid();
            Playing = true;
            Hand = Hand.HighCard;
            Cards = new List<Card>();
        }
    }

    
}
