using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonCardLibrary;

namespace Holdem.Services
{
    public class Table 
    {
        public Guid Id { get; set; }
        public virtual List<Player> Players { get; set; }
        public virtual Deck Deck { get; set; }
        public int Pot { get; set; }
        public virtual List<Card> Cards { get; set; }
        public bool DealOver { get; set; }
    }
}