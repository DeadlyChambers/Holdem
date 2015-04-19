using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCardLibrary
{
    public partial class Table
    {
        public List<Player> DetermineWinner()
        {
            foreach (var player in Players)
            {
                DetermineHand(player);
            }
            var bestHand = Players.GroupBy(x => (int)x.Hand).OrderByDescending(g => g.Key).First();
            var bestStrength =
                Players.Where(x => x.Hand == (Hand)bestHand.Key).GroupBy(x => x.HandStrength).OrderByDescending(g => g.Key).First();
            return Players.Where(x => x.Hand == (Hand)bestHand.Key && x.HandStrength == bestStrength.Key).ToList();

        }
    }
}
