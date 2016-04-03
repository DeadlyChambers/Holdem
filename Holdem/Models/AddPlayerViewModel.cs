using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CommonCardLibrary.Entities;

namespace Holdem.Models
{
    public class AddPlayerViewModel
    {
        public Guid TableId { get; set; }
        public Guid RoundId { get; set; }
        public Guid PlayerId { get; set; }
    }

    public class SubmitCardScreenViewModel : AddPlayerViewModel
    {
        public string Command { get; set; }
        public decimal RaiseAmount { get; set; } = (decimal) 0.00;
    }

    public class PlayerIdentityViewModel
    {
        public Guid Id { get; set; }
        [MinLength(6)]
        public string Username { get; set; }

        public decimal MoneyToAdd { get; set; }
    }

    public class DisplayPlayerOnIndexViewModel
    {
        public List<Player> Players { get; set; } 
        public Player Player { get; set; }
    }
}