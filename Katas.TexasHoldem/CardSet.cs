using System.Collections.Generic;
using System.Linq;

namespace Katas.TexasHoldem
{
    public class CardSet
    {
        protected readonly List<Card> _cards = new List<Card>(7);

        public CardSet(string handString)
        {
            var cardStrings = handString.Split(' ');
            cardStrings.ToList().ForEach(cardString => _cards.Add(new Card(cardString)));
        }

        public CardSet(IEnumerable<Card> cards)
        {
            _cards.AddRange(cards);
        }

        public IReadOnlyList<Card> Cards { get { return new List<Card>(_cards).AsReadOnly(); } }

        protected IReadOnlyList<Card> GetSet(IEnumerable<Card> cards, int setNumber)
        {
            return cards.OrderByDescending(card => card.Value).Skip(setNumber - 1).Take(5).ToList().AsReadOnly();
        }

        protected int NumberOfSets(IEnumerable<Card> cards)
        {
            return cards.Count() - 5 + 1;
        }
    }
}