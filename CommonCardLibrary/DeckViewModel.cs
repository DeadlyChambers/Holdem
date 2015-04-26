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
            Card tempCard;
            Random r = new Random(DateTime.Now.Millisecond);
            int randomNumber;
            for(int index = 51; index>0; index--)
            {
                randomNumber = r.Next(0, index + 1);
                tempCard = Cards[index];
                Cards[index] = Cards[randomNumber];
                Cards[randomNumber] = tempCard;
            }
        }

        public static List<Card> ToCards(this string cards)
        {
            var json = JArray.Parse(cards);
            return json.Select(p => new Card
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

        //public string ToJson()
        //{
        //    return new JObject
        //    {
        //        {"Cards", Cards.ToJson()}
        //    }.ToString();
        //}

        public static string ToString(this List<Card> Cards)
        {
            var str = "";
            foreach (Card card in Cards)
                if (card.Suit == Suit.Club)
                    str += card.Value.ToString() + " Club /n ";
                else if (card.Suit == Suit.Heart)
                    str += card.Value.ToString() + " Heart /n ";
                else if (card.Suit == Suit.Diamond)
                    str += card.Value.ToString() + " Diamond /n ";
                else
                    str += card.Value.ToString() + " Spade /n ";

            return str;
        }
    }    
}
