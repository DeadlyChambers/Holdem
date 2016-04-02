using System;
using System.Collections.Generic;

namespace CommonCardLibrary.Entities
{
    public class Table 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public decimal MinBet { get; set; }
        public decimal? MaxBet { get; set; }
        public decimal BuyIn { get; set; }
        public bool Active { get; set; }

        public List<Round> Rounds { get; set; } 
    }
}