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
        public byte? Position { get; set; }
       
        /// <summary>
        /// Json of the cards in hand
        /// </summary>
        public string Cards { get; set; }
        /// <summary>
        /// Cash the player has to bet from
        /// </summary>
        public decimal TotalCash { get; set; }
        /// <summary>
        /// Cash player has bet for this round
        /// </summary>
        public decimal? CashInHand { get; set; }
        /// <summary>
        /// Is the player currently acting
        /// </summary>
        public bool Acting { get; set; }
        /// <summary>
        /// If the player is currently in the hand, Waiting to act or has acted
        /// Should only be false if the player folded
        /// </summary>
        public bool Active { get; set; }
        
        /// <summary>
        /// If the player has won the game
        /// </summary>
        public bool Won { get; set; }
       
        public Round Round { get; set; }
        public Player Player { get; set; }
        
    }
}