using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Katas.TexasHoldem.Tests
{
    [TestFixture]
    public class PokerHandTests
    {
        [Test]
        public void CreateHand_SingleCard()
        {
            const string cardString = "4s";

            var hand = new PokerHand(cardString);

            Assert.That(hand.Cards.First(), Is.EqualTo(new Card(cardString)));
        }

        [Test]
        public void CreateHand_MultipleCards()
        {
            const string handString = "4s 5d";

            var hand = new PokerHand(handString);

            Assert.That(hand.Cards[0], Is.EqualTo(new Card("4s")));
            Assert.That(hand.Cards[1], Is.EqualTo(new Card("5d")));
        }

        [Test]
        [TestCase("4s 4d", 0, 4, 1)]
        [TestCase("3s 4s 4d", 1, 3, 2)]
        public void GetValueGroups_GroupsNumbersInDescendingOrderByHighestValue(string handString, int groupNumber, int expectedKey, int expectedGroupCount)
        {
            var hand = new PokerHand(handString);
            var groups = hand.GetValueGroups().ToArray();

            Assert.That(groups[groupNumber].Key, Is.EqualTo(expectedKey));
            Assert.That(groups.Count(), Is.EqualTo(expectedGroupCount));
        }

        [Test]
        [TestCase("4s 4d", 0, Faces.Spade, 2)]
        [TestCase("3s 4s 4d", 0, Faces.Spade, 2)]
        public void GetFaceGroups_GroupsFaces(string handString, int groupNumber, Faces expectedKey, int expectedGroupCount)
        {
            var hand = new PokerHand(handString);
            var groups = hand.GetFaceGroups().ToArray();

            Assert.That(groups[groupNumber].Key, Is.EqualTo(expectedKey));
            Assert.That(groups.Count(), Is.EqualTo(expectedGroupCount));
        }

        [Test]
        [TestCase("2s 4s 6s 8s Ts", true)]
        [TestCase("2s 4c 6d 8h Ts", false)]
        public void EvaluateForFlush_DetectsFlushWith5Cards(string handString, bool isFlushFound)
        {
            var hand = new PokerHand(handString);
            var result = hand.EvaluateForFlush();

            Assert.That(result.IsResultFound, Is.EqualTo(isFlushFound));
        }

        [Test]
        [TestCase("2s 4s 6s 8s Ts Qs As", Values.Ace)]
        [TestCase("Ts 4s 6s 8s 3s Qs 5s", Values.Queen)]
        public void EvaluateForFlush_SevenCardHand_HighestValueFlushFirst(string handString, int expectedHighCard)
        {
            var hand = new PokerHand(handString);
            var result = hand.EvaluateForFlush();

            var firstCardOfFirstHand = result.ListOfDiscoveredHands.First().Cards[0].Value;

            Assert.That(firstCardOfFirstHand, Is.EqualTo(expectedHighCard));
        }

        [Test]
        [TestCase("2s 3s 4s 5s 6s", true)]
        [TestCase("2s 4c 6d 8h Ts", false)]
        public void EvaluateFiveCardsForStraight_DetectsStraight(string handString, bool isFlushFound)
        {
            var hand = new PokerHand(handString);
            var result = hand.EvaluateFiveCardsForStraight(hand.Cards);

            Assert.That(result.IsResultFound, Is.EqualTo(isFlushFound));
        }

        [Test]
        [TestCase("2s 3s 4s 5s 6s", true)]
        [TestCase("2s 4c 6d 8h Ts", false)]
        public void EvaluateForStraight_DetectsStraightWith5Cards(string handString, bool isFlushFound)
        {
            var hand = new PokerHand(handString);
            var result = hand.EvaluateForStraight();

            Assert.That(result.IsResultFound, Is.EqualTo(isFlushFound));
        }

        [Test]
        [TestCase("2s 3s 4s 5s 6s 7s 8s", true, "8s", 3)]
        [TestCase("2s Ks 4s 5s 6s 7s 8s", true, "8s", 1)]
        public void EvaluateForStraight_DetectsMultipleStraightWith7Cards(string handString, bool isFlushFound, string expectedHighCard, int expectedDiscoveredHandCount)
        {
            var hand = new PokerHand(handString);
            var result = hand.EvaluateForStraight();

            Assert.That(result.IsResultFound, Is.EqualTo(isFlushFound));
            Assert.That(result.ListOfDiscoveredHands[0].Cards.First().Equals(new Card(expectedHighCard)));
            Assert.That(result.ListOfDiscoveredHands.Count, Is.EqualTo(expectedDiscoveredHandCount));
        }

        [Test]
        public void EvaluateForPair_DetectsPairWith5Cards()
        {
            var hand = new PokerHand("2s 2c 3s 4s 5s");

            HandResult result = hand.EvaluateForPair();

            Assert.That(result.IsResultFound, Is.EqualTo(true));
            Assert.That(result.ListOfDiscoveredHands.Count(), Is.EqualTo(1));
        }
    }
}
