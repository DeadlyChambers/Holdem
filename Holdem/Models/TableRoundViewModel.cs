using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using CommonCardLibrary.Entities;

namespace Holdem.Models
{
    public class TableRoundViewModel
    {
        public Table Table { get; set; }
        public Round Round { get; set; }
        public ReadOnlyCollection<Player> Players { get; set; } 
        public ReadOnlyCollection<Player> AvailablePlayers { get; set; } 
    }
}