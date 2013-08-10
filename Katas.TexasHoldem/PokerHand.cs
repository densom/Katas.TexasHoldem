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
            
            var overallResults = new StraightResult(false);
            var fiveCardHandResult = new StraightResult(false);

            var numberOfSets = NumberOfSets(Cards);

            for (int i = 0; i < numberOfSets; i++)
            {
                fiveCardHandResult = EvaluateFiveCardHandForStraight(i);
                AppendResults(overallResults, fiveCardHandResult);
            }

            return overallResults;
        }

        private static void AppendResults(StraightResult overallResults, StraightResult resultSet)
        {
            if (!overallResults.IsResultFound)
            {
                overallResults.IsResultFound = resultSet.IsResultFound;
            }

            if (resultSet.IsResultFound)
            {
                foreach (var discoveredHand in resultSet.ListOfDiscoveredHands)
                {
                    overallResults.AddDiscoveredHand(discoveredHand);
                }
            }
        }

        internal StraightResult EvaluateFiveCardHandForStraight(int setNumber)
        {
            var result = new StraightResult();

            var cardSet = GetSet(Cards, setNumber + 1);
            bool breakInSequenceDetected = false;

            for (int i = 1; i < 5; i++)
            {
                if (IsBreakInSequence(cardSet[i - 1], cardSet[i]))
                {
                    breakInSequenceDetected = true;
                    break;
                }
            }

            result.IsResultFound = !breakInSequenceDetected;
            
            if (!breakInSequenceDetected)
            {
                result.AddDiscoveredHand(new PokerHand(cardSet));
            }

            return result;
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