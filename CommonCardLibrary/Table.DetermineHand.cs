using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CommonCardLibrary
{
    public partial class TableViewModel
    {
        private int currentCardCount;
        private int tempHandStrength;
        private List<Card> cards;
        private Player _player;
        public void DetermineHand(Player player)
        {
            _player = player;
            cards = new List<Card>(_player.Cards);
            _player.HandStrength = 0;
            currentCardCount = 5;
            cards.AddRange(Cards);
            if (StraightFlush())
                return;
            if (FourOfAKind())
                return;
            if (FullHouse())
                return;
            if (_player.Hand == Hand.Flush || Flush())
                return;
            if (_player.Hand == Hand.Straight || Straight())
                return;
            if (_player.Hand == Hand.ThreeOfAKind || ThreeOfAKind())
                return;
            if (_player.Hand == Hand.Pair || _player.Hand == Hand.TwoPair || CheckPairs())
                return;
            HighCard();
        }

        public bool StraightFlush()
        {
            if (Flush() && Straight())
            {
                _player.Hand = Hand.StraightFlush;
                return true;
            }
            return false;
        }

        public bool FourOfAKind()
        {
            var multiples = cards.GroupBy(x => (int)x.Value).OrderByDescending(y => y.Key).FirstOrDefault(g => g.Count() == 4);
            if (multiples == null)
                return false;
            _player.Hand  = Hand.FourOfAKind;
            RemoveCardsOfValue(multiples.Key,4);
            return true;
        }

        public bool FullHouse()
        {
            if (ThreeOfAKind() && CheckPairs())
            {
                _player.Hand = Hand.FullHouse;
                return true;
            }
            return false;
        }

        public bool Flush()
        {
            var suits = Enum.GetValues(typeof(Suit));
            var flushCards = new List<Card>();
            foreach (Suit suit in suits)
            {
                 flushCards = cards.Where(x => x.Suit == suit).ToList();
                if (flushCards.Count() >= 5)
                    break;
            }
            if (flushCards.Count() < 5)
                return false;
            _player.Hand = Hand.Flush;
            cards = flushCards;
            GetValue(5);
            return true;
        }

        public bool Straight( )
        {
            var straight = cards.GroupBy(x => (int) x.Value).OrderByDescending(y => y.Key).ToList();
            var containsAce = cards.Count(x => x.Value == Value.Ace) > 0;
            var countForStraight = 0;
            var lastValue = straight[0].Key;
            foreach (var value in straight)
            {
                if (value.Key == lastValue)
                {
                    countForStraight++;
                    tempHandStrength += value.Key*13;
                }
                else
                {
                    if ((value.Key + 1) == lastValue)
                    {
                        countForStraight++;
                        tempHandStrength += value.Key*13;
                        
                        if (value.Key == 2 && containsAce && countForStraight!=5)
                        {
                            countForStraight++;
                            tempHandStrength += 13;
                        }
                    }
                    else
                    {
                        countForStraight = 1;
                        tempHandStrength = 0;
                    }
                    if (countForStraight == 5)
                    {
                        _player.Hand = Hand.Straight;
                        _player.HandStrength = tempHandStrength;
                        return true;
                    }
                    lastValue = value.Key;

                }
            }
            return false;
        }

        public bool ThreeOfAKind()
        {
            var trip = cards.GroupBy(x => (int)x.Value).OrderByDescending(y => y.Key).FirstOrDefault(g => g.Count() == 3);
            if (trip == null)
                return false;
            _player.Hand = Hand.ThreeOfAKind;
            RemoveCardsOfValue(trip.Key, 3);
            GetValue(currentCardCount);
            return true;
        }

        public bool HighCard()
        {
            GetValue(5);
            _player.Hand = Hand.HighCard;
            return true;
        }

        public bool CheckPairs( )
        {
            var multiples = cards.GroupBy(x => (int)x.Value).OrderByDescending(y => y.Key).Where(g => g.Count() >= 2).Select(ng => ng.Key).ToList();
            var pairCount = 2;
            switch (multiples.Count)
            {
                case 0:
                    return false;
                case 1:
                    _player.Hand = Hand.Pair;
                    break;
                default:
                    _player.Hand = Hand.TwoPair;
                    break;
            }

            foreach (var multiple in multiples)
            {
                RemoveCardsOfValue( multiple, 2);
                --pairCount;
                if (pairCount == 0)
                    break;
            }
            GetValue(currentCardCount);
            return true;
        }

        private void RemoveCardsOfValue(int pair, int decrementBy)
        {
            for (var x = cards.Count - 1; x > 0; x--)
                if (cards[x].Value == (Value) pair)
                    cards.RemoveAt(x);
            _player.HandStrength += pair*13*currentCardCount;
            currentCardCount -= decrementBy;
        }

        private void GetValue(int curCard)
        {
            foreach (var card in cards.OrderByDescending(x => (int)x.Value))
            {
                _player.HandStrength += (int) card.Value*13*curCard;
                curCard--;
                if (curCard == 0)
                    break;
            }
           
        }
    }
}
