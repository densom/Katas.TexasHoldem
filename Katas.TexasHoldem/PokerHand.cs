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
            
            //todo:  Test GREEN, but this next big time refactoring.

            var results = new StraightResult();
            var numberOfSets = NumberOfSets(Cards);

            for (int i = 0; i < numberOfSets; i++)
            {
                var cardSet = GetSet(Cards, i + 1);
                bool breakInSequenceDetected = false;

                for (int j = 1; j < 5; j++)
                {
                    if (IsBreakInSequence(cardSet[j - 1], cardSet[j]))
                    {
                        breakInSequenceDetected = true;
                        break;
                    }
                }

                // set IsResultsFound to false only if not already set.
                if (!results.IsResultFound)
                {
                    results.IsResultFound = !breakInSequenceDetected;    
                }

                
                if (!breakInSequenceDetected)
                {
                    results.AddDiscoveredHand(new PokerHand(cardSet));    
                }
            }

            return results;
        }

        private bool IsBreakInSequence(Card highCard, Card lowCard)
        {
            return highCard.Value - lowCard.Value != 1;
        }

        private IReadOnlyList<Card> GetSet(IEnumerable<Card> cards, int setNumber)
        {
            return cards.OrderByDescending(card => card.Value).Skip(setNumber - 1).Take(5).ToList().AsReadOnly();
        }

        private int NumberOfSets(IEnumerable<Card> cards)
        {
            return cards.Count() - 5 + 1;
        }
    }
}