using System;
using System.Collections.Generic;
using System.Linq;

namespace Katas.TexasHoldem
{
    public class PokerHand
    {
        private readonly List<Card> _cards = new List<Card>(7);

        public PokerHand(string handString)
        {
            var cardStrings = handString.Split(' ');
            cardStrings.ToList().ForEach(cardString => _cards.Add(new Card(cardString)));
        }

        public PokerHand(IEnumerable<Card> cards)
        {
            _cards.AddRange(cards);
        }

        public IReadOnlyList<Card> Cards { get { return new List<Card>(_cards).AsReadOnly(); } }

        internal IEnumerable<IGrouping<int, Card>> GetValueGroups()
        {
            return _cards.GroupBy(c => c.Value).OrderByDescending(x=>x.Key);
        }

        internal IEnumerable<IGrouping<Faces, Card>> GetFaceGroups()
        {
            return _cards.GroupBy(c => c.Face).OrderByDescending(x => x.Count());
        }

        public FlushResult EvaluateForFlush()
        {
            var faceGroupsWithMoreThan5Cards = GetFaceGroups().Where(group=>group.Count() >= 5);
            
            bool isFlushFound = faceGroupsWithMoreThan5Cards.Any();
            IEnumerable<PokerHand> listOfFlushHands = faceGroupsWithMoreThan5Cards.Select(x => new PokerHand(x.OrderByDescending(c=>c.Value).ToList()));

            return new FlushResult(isFlushFound, listOfFlushHands);   
        }

        public StraightResult EvaluateForStraight()
        {
            var fiveCardHand = Cards.OrderByDescending(card => card.Value).ToArray();

            for (int i = 1; i < 5; i++)
            {
                if (fiveCardHand[i-1].Value - fiveCardHand[i].Value != 1)
                {
                    return new StraightResult(false);
                }
            }


            return new StraightResult(true, new[] {new PokerHand(fiveCardHand), });
        }
    }
}