using System;

namespace Katas.TexasHoldem
{
    public class Card
    {
        public Card(string cardString)
        {
            Value = ParseValue(cardString[0]);
            Face = (Faces) cardString[1];
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

        public int Value { get; private set; }
        public Faces Face { get; private set; }
    }
}