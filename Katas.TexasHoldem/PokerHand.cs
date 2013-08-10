using System;
using System.Collections.Generic;
using System.Linq;

namespace Katas.TexasHoldem
{
    public class PokerHand
    {
        private readonly ICollection<Card> _cards = new List<Card>(7);

        public PokerHand(string handString)
        {
            var cardStrings = handString.Split(' ');
            cardStrings.ToList().ForEach(cardString => _cards.Add(new Card(cardString)));
        }

        public IReadOnlyList<Card> Cards { get { return new List<Card>(_cards).AsReadOnly(); } }

        public IEnumerable<IGrouping<int, Card>> GetValueGroups()
        {
            return _cards.GroupBy(c => c.Value).Select(x=>x).OrderByDescending(x=>x.Key);
        }

        public IEnumerable<IGrouping<Faces, Card>> GetFaceGroups()
        {
            return _cards.GroupBy(c => c.Face).Select(x => x).OrderByDescending(x => x.Count());
        }
    }
}