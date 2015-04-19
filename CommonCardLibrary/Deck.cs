using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCardLibrary
{
    public class Deck
    {
        public List<Card> Cards { get; set; }
        public Deck()
        {
            var suits = System.Enum.GetValues(typeof(Suit));
            var values = System.Enum.GetValues(typeof(Value));
            Cards = new List<Card>();
            foreach (Suit suit in suits)
                foreach (Value value in values)
                    Cards.Add(new Card(suit, value));
            
            Shuffle();
            Shuffle();
            Shuffle();
        }

        public void Shuffle()
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

        public override string ToString()
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
