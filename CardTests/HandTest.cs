using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using CommonCardLibrary;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CardTests
{
    [TestClass]
    public class HandTest
    {
        public PlayerViewModel GetPlayer(Card firstCard, Card secondCard)
        {
            var player = new PlayerViewModel();
            player.Cards.Add(firstCard);
            player.Cards.Add(secondCard);
            return player;
        }

        public TableViewModel GetTable(List<PlayerViewModel> players, List<Card> cards = null)
        {
            var deck = new List<Card>();
            deck.InitializeDeck();
            return new TableViewModel(deck, players, cards ?? new List<Card>());
        }


        [TestMethod]
        public void high_card_hand()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.Ten));
            var table = GetTable(new List<PlayerViewModel>() {player});
            table.DetermineHand(player);
            Assert.IsTrue(player.Hand == Hand.HighCard);
        }

        [TestMethod]
        public void ace_10_higher_than_king_10()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.Ten));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Ten));
            var table = GetTable(new List<PlayerViewModel>() { player, player2 });
            table.DetermineHand(player);
            table.DetermineHand(player2);
            Assert.IsTrue(player.HandStrength > player2.HandStrength);
        }

       

        [TestMethod]
        public void ace_ace_higher_than_ace_king()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.King));
            var table = GetTable(new List<PlayerViewModel>() { player, player2 });
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
        }

        [TestMethod]
        public void pair_hand()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.Ace));
            var table = GetTable(new List<PlayerViewModel>() { player });
            table.DetermineHand(player);
            Assert.IsTrue(player.Hand == Hand.Pair);
        }

        [TestMethod]
        public void not_pair_hand()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.King));
            var table = GetTable(new List<PlayerViewModel>() { player });
            table.DetermineHand(player);
            Assert.IsTrue(player.Hand != Hand.Pair);
        }

        [TestMethod]
        public void aces_beat_kings()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.King));
            var table = GetTable(new List<PlayerViewModel>() { player, player2 });
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
        }

        [TestMethod]
        public void twos_beat_Ace_King()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Two), new Card(Suit.Heart, Value.Two));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Ace));
            var table = GetTable(new List<PlayerViewModel>() { player, player2 });
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
        }

        [TestMethod]
        public void twos_tie_two_ties()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Two), new Card(Suit.Heart, Value.Two));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.Two), new Card(Suit.Heart, Value.Two));
            var table = GetTable(new List<PlayerViewModel>() { player, player2 });
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 2);
            Assert.IsTrue(winners.Count(x => x.Id == player.Id)==1);
            Assert.IsTrue(winners.Count(x => x.Id == player2.Id) == 1);
        }

        [TestMethod]
        public void twos_beat_Ace_King_Queen()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Two), new Card(Suit.Heart, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Ace));
            var cards = new List<Card>()
            {
                new Card(Suit.Heart, Value.Two)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
        }

        [TestMethod]
        public void twos_pair_hand()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Two), new Card(Suit.Heart, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Ace));
            var cards = new List<Card>()
            {
                new Card(Suit.Heart, Value.Two),
                new Card(Suit.Heart, Value.Ace)
            };
            var table = GetTable(new List<PlayerViewModel>() { player }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.TwoPair);
        }

        [TestMethod]
        public void twos_pair_hand_beats_pair()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Two), new Card(Suit.Heart, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Ace));
            var cards = new List<Card>()
            {
                new Card(Suit.Heart, Value.Two),
                new Card(Suit.Heart, Value.Ace)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.TwoPair);
        }

        [TestMethod]
        public void aces_over_kings_beats_aces_over_queens()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.Queen), new Card(Suit.Heart, Value.Ace));
            var cards = new List<Card>()
            {
                new Card(Suit.Heart, Value.King),
                new Card(Suit.Diamond, Value.Queen),
                new Card(Suit.Heart, Value.Ace)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.TwoPair);
        }

        [TestMethod]
        public void trips_hand()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Two), new Card(Suit.Heart, Value.Two));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Ace));
            var cards = new List<Card>()
            {
                new Card(Suit.Heart, Value.Two),
                new Card(Suit.Heart, Value.Ace)
            };
            var table = GetTable(new List<PlayerViewModel>() { player }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.ThreeOfAKind);
        }

        [TestMethod]
        public void trips_beat_two_pair()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Two), new Card(Suit.Heart, Value.Two));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.Two), new Card(Suit.Heart, Value.Ace));
            var cards = new List<Card>()
            {
                new Card(Suit.Heart, Value.Two),
                new Card(Suit.Heart, Value.Ace)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.ThreeOfAKind);
        }

        [TestMethod]
        public void trips_beat_pair()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Two), new Card(Suit.Heart, Value.Two));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Ace));
            var cards = new List<Card>()
            {
                new Card(Suit.Heart, Value.Two),
                new Card(Suit.Heart, Value.Ace)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.ThreeOfAKind);
        }

        [TestMethod]
        public void trips_beat_high_card()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Two), new Card(Suit.Heart, Value.Two));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Ace));
            var cards = new List<Card>()
            {
                new Card(Suit.Heart, Value.Two),
                new Card(Suit.Heart, Value.Three)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.ThreeOfAKind);
        }

        [TestMethod]
        public void trips_with_kicker_beat_trips()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Two), new Card(Suit.Heart, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Two));
            var cards = new List<Card>()
            {
                new Card(Suit.Heart, Value.Two),
                new Card(Suit.Heart, Value.Three),
                new Card(Suit.Diamond, Value.Two)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.ThreeOfAKind);
        }

        [TestMethod]
        public void trips_with_same_kicker_beat_ties()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Two), new Card(Suit.Heart, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.Two));
            var cards = new List<Card>()
            {
                new Card(Suit.Heart, Value.Two),
                new Card(Suit.Heart, Value.Three),
                new Card(Suit.Diamond, Value.Two)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 2);
            Assert.IsTrue(winners[0].Hand == Hand.ThreeOfAKind);
            Assert.IsTrue(winners[1].Hand == Hand.ThreeOfAKind);
            Assert.IsTrue(winners.Count(x => x.Id == player.Id) == 1);
            Assert.IsTrue(winners.Count(x => x.Id == player2.Id) == 1);
        }

        [TestMethod]
        public void straight_hand_ace_high()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.King));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Club, Value.Queen),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Heart, Value.Ten)
            };
            var table = GetTable(new List<PlayerViewModel>() { player }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.Straight);
        }

        [TestMethod]
        public void not_straight_hand_ace_high()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.King));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Club, Value.Queen),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Heart, Value.Ten)
            };
            var table = GetTable(new List<PlayerViewModel>() { player }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(player2.Hand == Hand.HighCard);
            Assert.IsTrue(player.Hand == Hand.Pair);
        }

        [TestMethod]
        public void straight_hand_ace_low()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Club, Value.Three),
                new Card(Suit.Heart, Value.Four),
                new Card(Suit.Heart, Value.Five)
            };
            var table = GetTable(new List<PlayerViewModel>() { player }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(player.WinningHand);
            Assert.IsTrue(player.Hand == Hand.Straight);
        }

        [TestMethod]
        public void straight_hand_ace_low_loses_to_two_low()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Six), new Card(Suit.Heart, Value.Two));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Club, Value.Three),
                new Card(Suit.Heart, Value.Four),
                new Card(Suit.Heart, Value.Five)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.Straight);
        }

        [TestMethod]
        public void straight_beats_trips()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Six), new Card(Suit.Heart, Value.Two));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Club, Value.Three),
                new Card(Suit.Heart, Value.Four),
                new Card(Suit.Heart, Value.Five),
                new Card(Suit.Diamond, Value.Ace),
                new Card(Suit.Club, Value.Ace)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.Straight);
        }

        [TestMethod]
        public void flush_hand()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.King));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Three),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Heart, Value.Ten)
            };
            var table = GetTable(new List<PlayerViewModel>() { player }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.Flush);
        }

        [TestMethod]
        public void flush_hand_ace_high_beats_king_high()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.Three));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Five),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Heart, Value.Ten)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.Flush);
        }

        [TestMethod]
        public void flush_hand_tie_on_board()
        {
            var player = GetPlayer(new Card(Suit.Club, Value.Four), new Card(Suit.Diamond, Value.Three));
            var player2 = GetPlayer(new Card(Suit.Spade, Value.King), new Card(Suit.Club, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Five),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Heart, Value.Ten),
                new Card(Suit.Heart, Value.Four),
                new Card(Suit.Heart, Value.Seven),

            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 2);
            Assert.IsTrue(winners[0].Hand == Hand.Flush);
            Assert.IsTrue(winners[1].Hand == Hand.Flush);
            Assert.IsTrue(winners.Count(x => x.Id == player.Id) == 1);
            Assert.IsTrue(winners.Count(x => x.Id == player2.Id) == 1);
        }

        [TestMethod]
        public void flush_win_with_kicker()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Six), new Card(Suit.Diamond, Value.Three));
            var player2 = GetPlayer(new Card(Suit.Spade, Value.King), new Card(Suit.Club, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Five),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Heart, Value.Ten),
                new Card(Suit.Heart, Value.Four),
                new Card(Suit.Heart, Value.Seven),

            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.Flush);
        }

        [TestMethod]
        public void flush_beats_straight()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Six), new Card(Suit.Diamond, Value.Three));
            var player2 = GetPlayer(new Card(Suit.Spade, Value.Six), new Card(Suit.Club, Value.Three));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Five),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Club, Value.Ten),
                new Card(Suit.Heart, Value.Four),
                new Card(Suit.Heart, Value.Seven),

            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.Flush);
            Assert.IsTrue(player2.Hand == Hand.Straight);
        }

        [TestMethod]
        public void flush_beats_trips()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Six), new Card(Suit.Diamond, Value.Three));
            var player2 = GetPlayer(new Card(Suit.Spade, Value.Five), new Card(Suit.Club, Value.Five));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Five),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Club, Value.Ten),
                new Card(Suit.Heart, Value.Four),
                new Card(Suit.Heart, Value.Seven),

            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.Flush);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.ThreeOfAKind);
        }

        [TestMethod]
        public void flush_beats_two_pair()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Six), new Card(Suit.Diamond, Value.Three));
            var player2 = GetPlayer(new Card(Suit.Spade, Value.Four), new Card(Suit.Club, Value.Five));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Five),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Club, Value.Ten),
                new Card(Suit.Heart, Value.Four),
                new Card(Suit.Heart, Value.Seven),

            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.Flush);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.TwoPair);
        }

        [TestMethod]
        public void flush_beats_pair()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Six), new Card(Suit.Diamond, Value.Three));
            var player2 = GetPlayer(new Card(Suit.Spade, Value.Ace), new Card(Suit.Club, Value.Five));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Five),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Club, Value.Ten),
                new Card(Suit.Heart, Value.Four),
                new Card(Suit.Heart, Value.Seven),

            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.Flush);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.Pair);
        }

        [TestMethod]
        public void flush_beats_high_card()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Six), new Card(Suit.Diamond, Value.Three));
            var player2 = GetPlayer(new Card(Suit.Spade, Value.Ace), new Card(Suit.Club, Value.Nine));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Five),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Club, Value.Ten),
                new Card(Suit.Heart, Value.Four),
                new Card(Suit.Heart, Value.Seven),

            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.Flush);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.HighCard);
        }

        [TestMethod]
        public void fullhouse_hand()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Diamond, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Club, Value.Ace),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Diamond, Value.Jack)
            };
            var table = GetTable(new List<PlayerViewModel>() { player }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.FullHouse);
        }

        [TestMethod]
        public void fullhouse_hand_with_two_threes()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Diamond, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Club, Value.Ace),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Diamond, Value.Jack),
                new Card(Suit.Club, Value.Jack)
            };
            var table = GetTable(new List<PlayerViewModel>() { player }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.FullHouse);
        }

        [TestMethod]
        public void fullhouse_aces_over_kings_beats_kings_over_aces()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Diamond, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Spade, Value.Ace));
            var cards = new List<Card>
            {
                new Card(Suit.Club, Value.Ace),
                new Card(Suit.Club, Value.King),
                new Card(Suit.Diamond, Value.King),
                new Card(Suit.Club, Value.Five),
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0] == player);
            Assert.IsTrue(winners[0].Hand == Hand.FullHouse);
        }

        [TestMethod]
        public void fullhouse_beats_flush()
        {
            var player = GetPlayer(new Card(Suit.Spade, Value.Ace), new Card(Suit.Diamond, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Ace),
                new Card(Suit.Club, Value.King),
                new Card(Suit.Diamond, Value.King),
                new Card(Suit.Heart, Value.Ten),
                new Card(Suit.Heart, Value.Jack)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.FullHouse);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.Flush);
        }

        [TestMethod]
        public void fullhouse_beats_straight()
        {
            var player = GetPlayer(new Card(Suit.Spade, Value.Ace), new Card(Suit.Diamond, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Diamond, Value.Queen), new Card(Suit.Heart, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Ace),
                new Card(Suit.Club, Value.King),
                new Card(Suit.Diamond, Value.King),
                new Card(Suit.Heart, Value.Ten),
                new Card(Suit.Heart, Value.Jack)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.FullHouse);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.Straight);
        }

        [TestMethod]
        public void fullhouse_beats_trips()
        {
            var player = GetPlayer(new Card(Suit.Spade, Value.Ace), new Card(Suit.Diamond, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Diamond, Value.King), new Card(Suit.Club, Value.Three));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Ace),
                new Card(Suit.Club, Value.King),
                new Card(Suit.Diamond, Value.King),
                new Card(Suit.Heart, Value.Ten),
                new Card(Suit.Heart, Value.Jack)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.FullHouse);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.ThreeOfAKind);
        }

        [TestMethod]
        public void fullhouse_beats_pair()
        {
            var player = GetPlayer(new Card(Suit.Spade, Value.Ace), new Card(Suit.Diamond, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Diamond, Value.Four), new Card(Suit.Club, Value.Three));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Ace),
                new Card(Suit.Club, Value.King),
                new Card(Suit.Diamond, Value.King),
                new Card(Suit.Heart, Value.Ten),
                new Card(Suit.Heart, Value.Jack)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.FullHouse);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.Pair);
        }

        [TestMethod]
        public void four_of_a_kind_hand()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Diamond, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Club, Value.Ace),
                new Card(Suit.Heart, Value.Ace),
                new Card(Suit.Diamond, Value.Jack)
            };
            var table = GetTable(new List<PlayerViewModel>() { player }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.FourOfAKind);
        }

        [TestMethod]
        public void four_aces_beats_four_kings()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Diamond, Value.Ace));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Diamond, Value.King));
            var cards = new List<Card>
            {
                new Card(Suit.Club, Value.Ace),
                new Card(Suit.Spade, Value.Ace),
                new Card(Suit.Spade, Value.King),
                new Card(Suit.Club, Value.King),
                new Card(Suit.Spade, Value.Four)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.FourOfAKind);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.FourOfAKind);
        }

        [TestMethod]
        public void str8_flush_hand()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.King));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Queen),
                new Card(Suit.Heart, Value.Ten),
                new Card(Suit.Heart, Value.Jack)
            };
            var table = GetTable(new List<PlayerViewModel>() { player }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.StraightFlush);
        }

        [TestMethod]
        public void str8_flush_beats_four_of_kind()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.King));
            var player2 = GetPlayer(new Card(Suit.Diamond, Value.Queen), new Card(Suit.Spade, Value.Queen));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Queen),
                new Card(Suit.Heart, Value.Ten),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Club, Value.Queen),
                new Card(Suit.Spade, Value.Six)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.StraightFlush);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.FourOfAKind);
        }

        [TestMethod]
        public void str8_flush_beats_FullHouse()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.King));
            var player2 = GetPlayer(new Card(Suit.Diamond, Value.Queen), new Card(Suit.Club, Value.Six));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Queen),
                new Card(Suit.Heart, Value.Ten),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Club, Value.Queen),
                new Card(Suit.Spade, Value.Six)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.StraightFlush);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.FullHouse);
        }

        [TestMethod]
        public void str8_flush_beats_Flush()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.King));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.Three), new Card(Suit.Heart, Value.Six));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Queen),
                new Card(Suit.Heart, Value.Ten),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Club, Value.Queen),
                new Card(Suit.Spade, Value.Six)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.StraightFlush);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.Flush);
        }

        [TestMethod]
        public void str8_flush_beats_straight()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.King));
            var player2 = GetPlayer(new Card(Suit.Club, Value.Ace), new Card(Suit.Diamond, Value.King));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Queen),
                new Card(Suit.Heart, Value.Ten),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Club, Value.Queen),
                new Card(Suit.Spade, Value.Six)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.StraightFlush);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.Straight);
        }

        [TestMethod]
        public void str8_flush_beats_trips()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.King));
            var player2 = GetPlayer(new Card(Suit.Club, Value.Ace), new Card(Suit.Diamond, Value.Queen));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Queen),
                new Card(Suit.Heart, Value.Ten),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Club, Value.Queen),
                new Card(Suit.Spade, Value.Six)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.StraightFlush);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.ThreeOfAKind);
        }

        [TestMethod]
        public void str8_flush_beats_pair()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.King));
            var player2 = GetPlayer(new Card(Suit.Club, Value.Four), new Card(Suit.Diamond, Value.Two));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Queen),
                new Card(Suit.Heart, Value.Ten),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Club, Value.Queen),
                new Card(Suit.Spade, Value.Six)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.StraightFlush);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.Pair);
        }

        [TestMethod]
        public void str8_flush_beats_two_pair()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.King));
            var player2 = GetPlayer(new Card(Suit.Club, Value.Five), new Card(Suit.Diamond, Value.Five));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Queen),
                new Card(Suit.Heart, Value.Ten),
                new Card(Suit.Heart, Value.Jack),
                new Card(Suit.Club, Value.Queen),
                new Card(Suit.Spade, Value.Six)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.StraightFlush);
            Assert.IsTrue(table.Players.First(x => x.Id == player2.Id).Hand == Hand.TwoPair);
        }

        [TestMethod]
        public void pair_on_board_two_pair_should_win()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.Five));
            var player2 = GetPlayer(new Card(Suit.Club, Value.Seven), new Card(Suit.Diamond, Value.Ace));
            var player3 = GetPlayer(new Card(Suit.Spade, Value.Jack), new Card(Suit.Spade, Value.Ten));
            var player4 = GetPlayer(new Card(Suit.Heart, Value.Two), new Card(Suit.Heart, Value.Queen));
            var player5 = GetPlayer(new Card(Suit.Diamond, Value.Nine), new Card(Suit.Spade, Value.Ace));
            var cards = new List<Card>
            {
                new Card(Suit.Diamond, Value.Eigth),
                new Card(Suit.Heart, Value.Three),
                new Card(Suit.Club, Value.Ten),
                new Card(Suit.Club, Value.Five),
                new Card(Suit.Club, Value.Three)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2, player3, player4, player5 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player3.Id);
            Assert.IsTrue(winners[0].Hand == Hand.TwoPair);
            
        }

//        2h6d
//7cJc
//Qd2d
//3d5c
//Fix the J thinks it is the high hand
        [TestMethod]
        public void fix_queen_beating_jack()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Two), new Card(Suit.Diamond, Value.Six));
            var player2 = GetPlayer(new Card(Suit.Club, Value.Seven), new Card(Suit.Club, Value.Jack));
            var player3 = GetPlayer(new Card(Suit.Diamond, Value.Queen), new Card(Suit.Diamond, Value.Two));
            var player4 = GetPlayer(new Card(Suit.Diamond, Value.Three), new Card(Suit.Club, Value.Five));
            var cards = new List<Card>
            {
               
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2, player3, player4}, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player3.Id);
            Assert.IsTrue(winners[0].Hand == Hand.HighCard);
        }

        [TestMethod]
        public void ace_king_higher_than_ace_queen()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.King));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.Ace), new Card(Suit.Heart, Value.Queen));
            var table = GetTable(new List<PlayerViewModel>() { player, player2 });
            table.DetermineHand(player);
            table.DetermineHand(player2);
            Assert.IsTrue(player.HandStrength > player2.HandStrength);
        }

        [TestMethod]
        public void ace_pair_beats_two_pair()
        {
//            2s 6h 4d 10s Ah
//3d 2d
//As 5h
            var player = GetPlayer(new Card(Suit.Diamond, Value.Three), new Card(Suit.Diamond, Value.Two));
            var player2 = GetPlayer(new Card(Suit.Spade, Value.Ace), new Card(Suit.Heart, Value.Five));
          
            var cards = new List<Card>
            {
                new Card(Suit.Spade, Value.Two),
                new Card(Suit.Heart, Value.Six),
                new Card(Suit.Diamond, Value.Four)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2}, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.Pair);
            Assert.IsTrue(player.WinningHand);

            table.TurnCardSpecific(new Card(Suit.Spade, Value.Ten));
            winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player.Id);
            Assert.IsTrue(winners[0].Hand == Hand.Pair);
            Assert.IsTrue(player.WinningHand);

            table.TurnCardSpecific(new Card(Suit.Heart, Value.Ace));
            winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(winners[0].Id == player2.Id);
            Assert.IsTrue(player2.Hand == Hand.Pair);
            Assert.IsTrue(player2.WinningHand);
            Assert.IsFalse(player.WinningHand);
        }

        [TestMethod]
        public void nines_were_not_winning_in_fourth()
        {
//            3c3s
//4sKd
//JcAd
//3d9s

//6s9h2s4d
            var player = GetPlayer(new Card(Suit.Club, Value.Three), new Card(Suit.Spade, Value.Three));
            var player2 = GetPlayer(new Card(Suit.Spade, Value.Four), new Card(Suit.Diamond, Value.King));
            var player3 = GetPlayer(new Card(Suit.Club, Value.Jack), new Card(Suit.Diamond, Value.Ace));
            var player4 = GetPlayer(new Card(Suit.Diamond, Value.Three), new Card(Suit.Spade, Value.Nine));
           
            var table = GetTable(new List<PlayerViewModel>() { player, player2, player3, player4 });
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(player.Hand == Hand.Pair);
            Assert.IsTrue(player.WinningHand);

            table.TurnCardSpecific(new Card(Suit.Spade, Value.Six),
                new Card(Suit.Heart, Value.Nine),
                new Card(Suit.Spade, Value.Two));
            winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(player4.Hand == Hand.Pair);
            Assert.IsFalse(player.WinningHand);
            Assert.IsTrue(player4.WinningHand);

            table.TurnCardSpecific(new Card(Suit.Diamond, Value.Four));
            winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(player.Hand == Hand.Pair);
            Assert.IsTrue(player4.WinningHand);

            table.TurnCardSpecific(new Card(Suit.Heart, Value.Ace));
            winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 1);
            Assert.IsTrue(player3.Hand == Hand.Pair);
            Assert.IsTrue(player3.WinningHand);
            Assert.IsFalse(player.WinningHand);
            Assert.IsFalse(player4.WinningHand);

        }

        [TestMethod]
        public void eights_should_win_with_straight_onboard()
        {
            var player = GetPlayer(new Card(Suit.Diamond, Value.Ten), new Card(Suit.Heart, Value.Eigth));
            var player2 = GetPlayer(new Card(Suit.Club, Value.Eigth), new Card(Suit.Spade, Value.King));
            var player3 = GetPlayer(new Card(Suit.Club, Value.King), new Card(Suit.Diamond, Value.King));
            var player4 = GetPlayer(new Card(Suit.Club, Value.Six), new Card(Suit.Spade, Value.Five));
            var cards = new List<Card>
            {
                new Card(Suit.Heart, Value.Five),
                new Card(Suit.Diamond, Value.Four),
                new Card(Suit.Spade, Value.Six),
                new Card(Suit.Diamond, Value.Three),
                new Card(Suit.Spade, Value.Seven)
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2, player3, player4 }, cards);
            var winners = table.DetermineWinner();
            Assert.IsTrue(winners.Count == 2);
            Assert.IsTrue(player.Hand == Hand.Straight);
            Assert.IsTrue(player2.Hand == Hand.Straight);
            Assert.IsTrue(player.WinningHand);
            Assert.IsTrue(player2.WinningHand);
            Assert.IsFalse(player3.WinningHand);
            Assert.IsFalse(player4.WinningHand);
        }

        [TestMethod]
        public void ace_over_jacks_beats_qs_over_nines()
        {
            var player = GetPlayer(new Card(Suit.Heart, Value.King), new Card(Suit.Heart, Value.Five));
            var player2 = GetPlayer(new Card(Suit.Diamond, Value.Ten), new Card(Suit.Club, Value.Ten));
            var player3 = GetPlayer(new Card(Suit.Spade, Value.Ace), new Card(Suit.Spade, Value.Jack));
            var player4 = GetPlayer(new Card(Suit.Spade, Value.Nine), new Card(Suit.Spade, Value.Queen));
            var cards = new List<Card>
            {
                new Card(Suit.Club, Value.Queen),
                new Card(Suit.Diamond, Value.Three),
                new Card(Suit.Heart, Value.Ace),
              
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2, player3, player4 }, cards);
            table.DetermineWinner();
            Assert.IsTrue(player2.Hand == Hand.Pair);
            Assert.IsTrue(player3.Hand == Hand.Pair);
            Assert.IsTrue(player4.Hand == Hand.Pair);
            Assert.IsTrue(player3.WinningHand);

            table.TurnCardSpecific(new Card(Suit.Club, Value.Jack));
            table.DetermineWinner();
            Assert.IsTrue(player2.Hand == Hand.Pair);
            Assert.IsTrue(player3.Hand == Hand.TwoPair);
            Assert.IsTrue(player4.Hand == Hand.Pair);
            Assert.IsTrue(player3.WinningHand);

            table.TurnCardSpecific(new Card(Suit.Spade, Value.Three));
            table.DetermineWinner();
            Assert.IsTrue(player3.WinningHand);
            Assert.IsTrue(player2.Hand == Hand.TwoPair);
            Assert.IsTrue(player3.Hand == Hand.TwoPair);
            Assert.IsTrue(player4.Hand == Hand.TwoPair);
        }

        [TestMethod]
        public void higher_overs_on_fullhouse_win()
        {
            var player = GetPlayer(new Card(Suit.Diamond, Value.Two), new Card(Suit.Spade, Value.Two));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.Three), new Card(Suit.Heart, Value.Queen));
            var cards = new List<Card>
            {
                new Card(Suit.Club, Value.Nine),
                new Card(Suit.Diamond, Value.Three),
                new Card(Suit.Heart, Value.Two),
                new Card(Suit.Club, Value.Three),
                new Card(Suit.Heart, Value.Nine)
              
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            table.DetermineWinner();
            Assert.IsTrue(player.Hand == Hand.FullHouse);
            Assert.IsTrue(player2.Hand == Hand.FullHouse); 
            Assert.IsTrue(player2.WinningHand);
            Assert.IsFalse(player.WinningHand);
        }

        [TestMethod]
        public void higher_unders_on_fullhouse_win()
        {
            var player = GetPlayer(new Card(Suit.Diamond, Value.Nine), new Card(Suit.Spade, Value.Two));
            var player2 = GetPlayer(new Card(Suit.Heart, Value.Nine), new Card(Suit.Heart, Value.Three));
            var cards = new List<Card>
            {
                new Card(Suit.Club, Value.Nine),
                new Card(Suit.Diamond, Value.Four),
                new Card(Suit.Heart, Value.Two),
                new Card(Suit.Club, Value.Three),
                new Card(Suit.Heart, Value.Nine)
              
            };
            var table = GetTable(new List<PlayerViewModel>() { player, player2 }, cards);
            table.DetermineWinner();
            Assert.IsTrue(player.Hand == Hand.FullHouse);
            Assert.IsTrue(player2.Hand == Hand.FullHouse);
            Assert.IsTrue(player2.WinningHand);
            Assert.IsFalse(player.WinningHand);
        }
      
    }
}
