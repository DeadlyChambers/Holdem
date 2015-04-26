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
       
        private List<Card> cards;
        private PlayerViewModel _playerViewModel;
        public void DetermineHand(PlayerViewModel playerViewModel)
        {
            _playerViewModel = playerViewModel;
            cards = new List<Card>(_playerViewModel.Cards);
            _playerViewModel.HandStrength = 0;
            _playerViewModel.Hand = Hand.HighCard;
            
            currentCardCount = 5;
            cards.AddRange(Cards);
            if (StraightFlush())
                return;
            if (FourOfAKind())
                return;
            if (FullHouse())
                return;
            if (_playerViewModel.Hand == Hand.Flush || Flush())
                return;
            if (_playerViewModel.Hand == Hand.Straight || Straight())
                return;
            if (_playerViewModel.Hand == Hand.ThreeOfAKind || ThreeOfAKind())
                return;
            if(_playerViewModel.Hand == Hand.TwoPair || CheckPairs())
                return;
            if (_playerViewModel.Hand == Hand.Pair || CheckPairs())
                return;
            HighCard();
        }

        public bool StraightFlush()
        {
            if (Flush() && Straight())
            {
                _playerViewModel.Hand = Hand.StraightFlush;
                return true;
            }
            return false;
        }

        public bool FourOfAKind()
        {
            var multiples = cards.GroupBy(x => (int)x.Value).OrderByDescending(y => y.Key).FirstOrDefault(g => g.Count() == 4);
            if (multiples == null)
                return false;
            _playerViewModel.Hand  = Hand.FourOfAKind;
            RemoveCardsOfValue(multiples.Key,4);
            return true;
        }

        public bool FullHouse()
        {
            if (ThreeOfAKind() && CheckPairs())
            {
                _playerViewModel.Hand = Hand.FullHouse;
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
            _playerViewModel.Hand = Hand.Flush;
            cards = flushCards;
            GetValue(5);
            return true;
        }

        public bool Straight( )
        {
            var containsAce = cards.Count(x => x.Value == Value.Ace) > 0;
            var straight = cards.GroupBy(x => (int) x.Value).OrderByDescending(y => y.Key).ToList();
            var straightValues = straight.Select((v, c) => v.Key).ToList();
            if (containsAce)
                straightValues.Add(1);
            var countForStraight = 0;
            var startvalue = straight[0].Key;
            var tempHandStrength = 0;
            foreach (var val in straightValues)
            {
                if (val == startvalue - countForStraight)
                {
                    countForStraight++;
                    tempHandStrength += val*13;
                }
                else
                {
                    startvalue = val;
                    countForStraight = 1;
                    tempHandStrength = val * 13;
                }
                if (countForStraight == 5)
                {
                    _playerViewModel.Hand = Hand.Straight;
                    _playerViewModel.HandStrength = tempHandStrength;
                    return true;
                }
            }
            return false;
        }

        public bool ThreeOfAKind()
        {
            var trip = cards.GroupBy(x => (int)x.Value).OrderByDescending(y => y.Key).FirstOrDefault(g => g.Count() == 3);
            if (trip == null)
                return false;
            _playerViewModel.Hand = Hand.ThreeOfAKind;
            RemoveCardsOfValue(trip.Key, 3);
            GetValue(currentCardCount);
            return true;
        }

        public bool HighCard()
        {
            GetValue(5);
            _playerViewModel.Hand = Hand.HighCard;
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
                    _playerViewModel.Hand = Hand.Pair;
                    break;
                default:
                    _playerViewModel.Hand = Hand.TwoPair;
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
            _playerViewModel.HandStrength += pair*(int)Math.Pow(13,currentCardCount);
            currentCardCount -= decrementBy;
        }

        private void GetValue(int curCard)
        {
            var newOrder = cards.OrderByDescending(x => (int) x.Value);
            foreach (var card in newOrder)
            {
                _playerViewModel.HandStrength += (int) card.Value*(int)Math.Pow(13,curCard);
                curCard--;
                if (curCard == 0)
                    break;
            }
           
        }
    }
}
