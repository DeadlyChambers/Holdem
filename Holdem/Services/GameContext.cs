using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CommonCardLibrary;
using CommonCardLibrary.Entities;

namespace Holdem.Services
{
    public class GameContext : DbContext
    {
        public DbSet<Round> Rounds { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerHand> PlayerHands { get; set; }
        //public DbSet<Game> Games { get; set; } 
        public GameContext()
            : base("HoldemDBConnectionString")
        {
            Database.SetInitializer<GameContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelB)
        {
            modelB.HasDefaultSchema("dbo");
            modelB.Entity<Round>().ToTable("Rounds");
            modelB.Entity<Table>().ToTable("Tables");
            modelB.Entity<Player>().ToTable("Players");
            modelB.Entity<PlayerHand>().ToTable("PlayerHands");
        }
    }
}