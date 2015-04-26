using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CommonCardLibrary;

namespace Holdem.Services
{
    public static class GameStuff
    {
        public static Guid GameId { get; set; }
    }

    public class GameContext : DbContext
    {
        public DbSet<Table> Tables { get; set; }
        public DbSet<Player> Players { get; set; }
        //public DbSet<Game> Games { get; set; } 
        public GameContext()
            : base("name=LocalHoldemDBConnectionString")
        {
        }
    }
}