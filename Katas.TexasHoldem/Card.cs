using System;

namespace Katas.TexasHoldem
{
    public class Card : IEquatable<Card>
    {
        public Card(string cardString)
        {
            Value = ParseValue(cardString[0]);
            Face = (Faces) cardString[1];
        }

        public int Value { get; private set; }
        public Faces Face { get; private set; }
        
        public bool Equals(Card other)
        {
            return Value == other.Value && Face == other.Face;
        }

        private static int ParseValue(char cardString)
        {
            if (char.IsDigit(cardString))
            {
                return int.Parse(cardString.ToString());
            }

            switch (cardString)
            {
                case 'T':
                    return 10;
                case 'J':
                    return 11;
                case 'Q':
                    return 12;
                case 'K':
                    return 13;
                case 'A':
                    return 14;
                default:
                    return 0;
            }
            
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Value, Face);
        }
    }
}