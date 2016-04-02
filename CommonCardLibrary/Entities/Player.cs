using System;
using System.Collections.Generic;

namespace CommonCardLibrary.Entities
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Cash { get; set; }

        public virtual List<PlayerHand> Hands { get; set; } 

    }
}