using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CommonCardLibrary
{
    public class Card
    {
        public Card()
        {
        }

        public Card(Suit suit, Value value)
        {
            Suit = suit;
            Value = value;
        }

        public Value Value { get; set; }
        public Suit Suit { get; set; }
        public bool IsDown { get; set; }
        public bool IsSecondCard { get; set; }

        public string SuitUrl()
        {
            if (Suit == Suit.Club)
                return "~/Content/Images/club.png";
            if (Suit == Suit.Spade)
                return "~/Content/Images/spade.png";
            if (Suit == Suit.Diamond)
                return "~/Content/Images/diamond.png";
            return "~/Content/Images/heart.png";
        }

        public string ValueToString()
        {
            if (Value == Value.Ace)
                return "A";
            if (Value == Value.King)
                return "K";
            if (Value == Value.Queen)
                return "Q";
            if (Value == Value.Jack)
                return "J";
            return ((int) Value).ToString();
        }
    }

   
}
