using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CommonCardLibrary
{
    public static class DeckActions
    {
        public static void InitializeDeck(this List<Card> Cards)
        {
            var suits = Enum.GetValues(typeof(Suit));
            var values = Enum.GetValues(typeof(Value));
            Cards.AddRange(from Suit suit in suits from Value value in values select new Card(suit, value));
            Cards.Shuffle();
            Cards.Shuffle();
            Cards.Shuffle();
        } 

        public static void Shuffle(this List<Card> Cards)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            for(int index = 51; index>0; index--)
            {
                 var randomNumber = r.Next(0, index + 1);
                var tempCard = Cards[index];
                Cards[index] = Cards[randomNumber];
                Cards[randomNumber] = tempCard;
            }
        }

        public static List<Card> ToCards(this string cards)
        {
            return JArray.Parse(cards).Select(p => new Card
            {
                Value = EnumParse.ValueDictionary[p["Value"].ToString()],
                Suit = EnumParse.SuitDictionary[p["Suit"].ToString()],
                IsDown = (bool)p["IsDown"],
                IsSecondCard = (bool)p["IsSecondCard"]
            }).ToList();

        }

        public static string ToJson(this List<Card> cards)
        {
            return new JArray(
                cards.Select(p => new JObject
                {
                    {"Value", (int) p.Value},
                    {"Suit", (int) p.Suit},
                    {"IsDown", p.IsDown},
                    {"IsSecondCard", p.IsSecondCard}
                })
                ).ToString();
        }
       
        public static string ToString(this List<Card> Cards)
        {
            var str = new StringBuilder();
            foreach (Card card in Cards)
                if (card.Suit == Suit.Club)
                    str.AppendLine(card.Value + " Club ");
                else if (card.Suit == Suit.Heart)
                    str.AppendLine(card.Value + " Heart ");
                else if (card.Suit == Suit.Diamond)
                    str.AppendLine(card.Value+ " Diamond ");
                else
                    str.AppendLine(card.Value + " Spade ");
            return str.ToString();
        }
    }    
}
