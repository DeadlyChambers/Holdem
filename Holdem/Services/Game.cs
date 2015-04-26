using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holdem.Services
{
    public class Game
    {
        public Guid Id { get; set; }
        public virtual List<Player> Players
        {
            get; set; }
        public virtual List<Table> Tables { get; set; } 

    }
}