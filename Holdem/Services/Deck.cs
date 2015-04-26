using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holdem.Services
{
    public class Deck
    {
        public Guid Id { get; set; }
        public string CardsJson { get; set; }
    }
}