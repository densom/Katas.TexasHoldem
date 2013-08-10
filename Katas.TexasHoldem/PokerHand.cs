using System;
using System.Collections.Generic;
using System.Linq;

namespace Katas.TexasHoldem
{
    public class PokerHand : CardSet
    {
        public PokerHand(string handString)
            : base(handString)
        {
        }

        public PokerHand(IEnumerable<Card> cards)
            : base(cards)
        {
        }

        internal static IEnumerable<IGrouping<int, Card>> GetValueGroups(IReadOnlyList<Card> cards)
        {
            return cards.GroupBy(c => c.Value).OrderByDescending(x => x.Key);
        }

        internal static IEnumerable<IGrouping<Faces, Card>> GetFaceGroups(IReadOnlyList<Card> cards)
        {
            return cards.GroupBy(c => c.Face).OrderByDescending(x => x.Count());
        }

        public HandResult EvaluateForFlush()
        {
            var faceGroupsWithMoreThan5Cards = GetFaceGroups(Cards).Where(group => group.Count() >= 5);

            bool isFlushFound = faceGroupsWithMoreThan5Cards.Any();
            IEnumerable<PokerHand> listOfFlushHands = faceGroupsWithMoreThan5Cards.Select(x => new PokerHand(x.OrderByDescending(c => c.Value).ToList()));

            return new HandResult(isFlushFound, listOfFlushHands);
        }

        public HandResult EvaluateForStraight()
        {

            var overallResults = new HandResult(false);
            var fiveCardHandResult = new HandResult(false);

            var numberOfSets = NumberOfSets(Cards);

            for (int i = 0; i < numberOfSets; i++)
            {
                fiveCardHandResult = EvaluateFiveCardsForStraight(GetSet(Cards, i + 1));
                AppendResults(overallResults, fiveCardHandResult);
            }

            return overallResults;
        }

        private static void AppendResults(HandResult overallResults, HandResult resultSet)
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

        internal HandResult EvaluateFiveCardsForStraight(IReadOnlyList<Card> cards)
        {
            var result = new HandResult();
            var orderedCards = cards.OrderByDescending(c => c.Value).ToArray();

            bool breakInSequenceDetected = false;

            for (int i = 1; i < 5; i++)
            {
                if (IsBreakInSequence(orderedCards[i - 1], orderedCards[i]))
                {
                    breakInSequenceDetected = true;
                    break;
                }
            }

            result.IsResultFound = !breakInSequenceDetected;

            if (!breakInSequenceDetected)
            {
                result.AddDiscoveredHand(new PokerHand(orderedCards));
            }

            return result;
        }

        private bool IsBreakInSequence(Card highCard, Card lowCard)
        {
            return highCard.Value - lowCard.Value != 1;
        }


        public HandResult EvaluateOfAKind(int numberOfCardsToMatch)
        {
            var results = new HandResult();

            var pairValueGroups = GetValueGroups(Cards).Where(g => g.Count() == numberOfCardsToMatch);

            foreach (var pairValueGroup in pairValueGroups)
            {
                results.IsResultFound = true;
                results.AddDiscoveredHand(new CardSet(pairValueGroup.ToArray()));
            }

            return results;
        }


        private HandResult EvaluateFiveCardsForStraightFlush(IEnumerable<Card> cards )
        {
            if (EvaluateForStraight().IsResultFound && EvaluateForFlush().IsResultFound)
            {
                var result = new HandResult(true);
                result.AddDiscoveredHand(new CardSet(cards));
                return result;
            }

            return new HandResult(false);
        }

        public HandResult EvaluateForStraightFlush()
        {
            var overallResult = new HandResult(false);
            
            overallResult.IsResultFound = EvaluateForStraight().IsResultFound && EvaluateForFlush().IsResultFound;
            
            var setCount = NumberOfSets(Cards);

            for (int i = 0; i < setCount; i++)
            {
                var set = GetSet(Cards, i);
                var result = EvaluateFiveCardsForStraightFlush(set);
                if (result.IsResultFound)
                {
                    result.ListOfDiscoveredHands.ToList().ForEach(overallResult.AddDiscoveredHand);
                }

            }
            
            return overallResult;
        }

        public HandResult EvaluateForRoyalFlush()
        {
            var straightFlushResult = EvaluateForStraightFlush();

            if (straightFlushResult.IsResultFound)
            {
                foreach (var hand in straightFlushResult.ListOfDiscoveredHands)
                {
                    if (IsHandHasAnAce(hand))
                    {
                        return new HandResult(true, new[] {new PokerHand(hand.Cards), });
                    }
                }
            }

            return new HandResult(false);
        }

        private static bool IsHandHasAnAce(CardSet hand)
        {
            return hand.Cards.Any(card => card.Value == Values.Ace);
        }
    }
}