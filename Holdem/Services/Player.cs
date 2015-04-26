using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holdem.Services
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Playing { get; set; }
        public int Position { get; set; }
        public string CardsJson { get; set; }
        public int Chips { get; set; }
        public int HandEnum { get; set; }
        public int HandStrength { get; set; }
        public bool WinningHand { get; set; }
    }
}