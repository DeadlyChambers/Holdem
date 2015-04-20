using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Holdem.Services
{
    public class GameContext : DbContext
    {
        public DbSet<Table> Tables { get; set; } 
    }
}