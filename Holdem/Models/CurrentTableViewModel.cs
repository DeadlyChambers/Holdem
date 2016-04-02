using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonCardLibrary.Entities;

namespace Holdem.Models
{
    public class CurrentTableViewModel
    {
        public Table Table { get; set; }
        public Round Round { get; set; }
        public PlayerHand Player { get; set; }
        public string Name { get; set; }

    }
}