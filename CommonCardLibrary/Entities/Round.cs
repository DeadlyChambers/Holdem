using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonCardLibrary.Entities
{
    public class Round
    {
        public Guid Id { get; set; }
        [ForeignKey("Table")]
        public Guid TableId { get; set; }

        /// <summary>
        /// JSON of the flop, 4th, and the river
        /// </summary>
        public string Cards { get; set; }

        /// <summary>
        /// Total Pot for hand
        /// </summary>
        public decimal Pot { get; set; } = (decimal)0.00;

        /// <summary>
        /// Keeping record of non active game means it is over
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// At beginning of the round 
        /// </summary>
        public byte Dealer { get; set; } = 1;

        /// <summary>
        /// There can only be one game that hasn't started for a table
        /// </summary>
        public bool Started { get; set; }

        /// <summary>
        /// The place we currently are in the round PreFlop,Flop, etc...
        /// </summary>
        public TableRound Place { get; set; }
        public List<PlayerHand> Players { get; set; } 

        public Table Table { get; set; }

        /// <summary>
        /// The current bet on the table. This is the HIGHEST bet on the table
        /// It will likely happen that someone raises above what the current and
        /// all players will have to meet it, beat it, all in, or fold
        /// </summary>
        public decimal CurrentBet { get; set; } = (decimal) 0.00;

    }
}