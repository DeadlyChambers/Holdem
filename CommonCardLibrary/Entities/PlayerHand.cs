using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonCardLibrary.Entities
{
    public class PlayerHand
    {
        [Column(Order=0),Key, ForeignKey("Player")]
        public Guid PlayerId { get; set; }
        [Column(Order = 1), Key, ForeignKey("Round")]
        public Guid RoundId { get; set; }

        /// <summary>
        /// Players position max of 8
        /// </summary>
        public byte Position { get; set; } = 1;
       
        /// <summary>
        /// Json of the cards in hand
        /// </summary>
        public string Cards { get; set; }

        /// <summary>
        /// Cash the player has to bet from
        /// </summary>
        public decimal TotalCash { get; set; } = (decimal) 0.00;
      
        /// <summary>
        /// Is the player currently acting
        /// </summary>
        public bool Acting { get; set; }

        /// <summary>
        /// If the player is currently in the hand, Waiting to act or has acted
        /// Should only be false if the player folded
        /// </summary>
        public bool Active { get; set; } = true;
        
        /// <summary>
        /// If the player has won the game
        /// </summary>
        public bool Won { get; set; }
       
        public Round Round { get; set; }
        public Player Player { get; set; }

        [NotMapped]
        public string Name { get; set; }

        /// <summary>
        /// The player's total bet
        /// </summary>
        public decimal CurrentBet { get; set; } = (decimal) 0.00;

        /// <summary>
        /// The player's total bet
        /// </summary>
        public decimal CashIn { get; set; } = (decimal)0.00;

        /// <summary>
        /// If the player is all in they will have no need to make a turn
        /// </summary>
        public bool AllIn { get; set; }

        /// <summary>
        /// When you are a new entry you might show up in the middle of the round and need to wait
        /// </summary>
        public bool Waiting { get; set; }
    }
}