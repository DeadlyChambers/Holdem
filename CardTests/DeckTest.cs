using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonCardLibrary;
using System.Linq;

namespace CardTests
{
    [TestClass]
    public class DeckTest
    {
        [TestMethod]
        public void ensure_deck_has_52_cards()
        {
            var deck = new Deck();
            Assert.IsTrue(deck.Cards.Count == 52);
            Console.WriteLine(deck.ToString());
         
        }

        [TestMethod]
        public void ensure_deck_has_correct_number_of_suits()
        {
            var deck = new Deck();
            Assert.IsTrue(deck.Cards.Count(x => x.Suit == Suit.Club) == 13);
            Assert.IsTrue(deck.Cards.Count(x => x.Suit == Suit.Diamond) == 13);
            Assert.IsTrue(deck.Cards.Count(x => x.Suit == Suit.Heart) == 13);
            Assert.IsTrue(deck.Cards.Count(x => x.Suit == Suit.Spade) == 13);
        }
    }
}
