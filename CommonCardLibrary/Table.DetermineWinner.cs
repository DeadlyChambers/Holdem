using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCardLibrary
{
    public partial class TableViewModel
    {
        public List<PlayerViewModel> DetermineWinner()
        {
            foreach (var player in Players)
            {
                DetermineHand(player);
            }
            var bestHand = Players.GroupBy(x => (int)x.Hand).OrderByDescending(g => g.Key).First();
            var bestStrength =
                Players.Where(x => x.Hand == (Hand)bestHand.Key).GroupBy(x => x.HandStrength).OrderByDescending(g => g.Key).First();
            var leaders =
                Players.Where(x => x.Hand == (Hand) bestHand.Key && x.HandStrength == bestStrength.Key).ToList();
            foreach (var player in Players)
            {
                player.WinningHand = leaders.Count(x => x.Id == player.Id) == 1;
            }
            return leaders;

        }
    }
}
