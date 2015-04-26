using System.Collections.Generic;

namespace CommonCardLibrary
{
   

    public enum Suit
    {
        Heart = 1,
        Diamond =2,
        Spade=3,
        Club=4
    }

    public enum Value
    {
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eigth = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    }

    public enum Hand
    {
        HighCard = 1,
        Pair = 2,
        TwoPair = 3,
        ThreeOfAKind = 4,
        Straight = 5,
        Flush = 6,
        FullHouse = 7,
        FourOfAKind = 8,
        StraightFlush = 9
    }

    public static class EnumParse
    {
        public static readonly Dictionary<string, Suit> SuitDictionary
            = new Dictionary<string, Suit>
            {
                {"1", Suit.Heart},
                {"2", Suit.Diamond},
                {"3", Suit.Spade},
                {"4", Suit.Club}
            };

        public static readonly Dictionary<string, Value> ValueDictionary
            = new Dictionary<string, Value>
            {
                {"2", Value.Two},
                {"3", Value.Three},
                {"4", Value.Four},
                {"5", Value.Five},
                {"6", Value.Six},
                {"7", Value.Seven},
                {"8", Value.Eigth},
                {"9", Value.Nine},
                {"10", Value.Ten},
                {"11", Value.Jack},
                {"12", Value.Queen},
                {"13", Value.King},
                {"14", Value.Ace}

            };

        public static readonly Dictionary<string, Hand> HandDictionary
            = new Dictionary<string, Hand>
            {
                {"1", Hand.HighCard},
                {"2", Hand.Pair},
                {"3", Hand.TwoPair},
                {"4", Hand.ThreeOfAKind},
                {"5", Hand.Straight},
                {"6", Hand.Flush},
                {"7", Hand.FullHouse},
                {"8", Hand.FourOfAKind},
                {"9", Hand.StraightFlush},
            };
    }
}
