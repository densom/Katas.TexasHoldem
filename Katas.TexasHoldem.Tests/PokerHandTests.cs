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
            var groups = PokerHand.GetValueGroups(hand.Cards).ToArray();

            Assert.That(groups[groupNumber].Key, Is.EqualTo(expectedKey));
            Assert.That(groups.Count(), Is.EqualTo(expectedGroupCount));
        }

        [Test]
        [TestCase("4s 4d", 0, Faces.Spade, 2)]
        [TestCase("3s 4s 4d", 0, Faces.Spade, 2)]
        public void GetFaceGroups_GroupsFaces(string handString, int groupNumber, Faces expectedKey, int expectedGroupCount)
        {
            var hand = new PokerHand(handString);
            var groups = PokerHand.GetFaceGroups(hand.Cards).ToArray();

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
        [TestCase("2s 2c 3s 4s 5s", true, 1)]
        [TestCase("2s 2c 3s 3s 5s", true, 2)]
        public void EvaluateOfAKind_DetectsPairWith5Cards(string handString, bool isPairFound, int expectedGroupsDiscovered)
        {
            var hand = new PokerHand(handString);

            HandResult result = hand.EvaluateOfAKind(2);

            Assert.That(result.IsResultFound, Is.EqualTo(isPairFound));
            Assert.That(result.ListOfDiscoveredHands.Count(), Is.EqualTo(expectedGroupsDiscovered));
        }

        [Test]
        [TestCase("2s 2c 3s 4s 5s 6s 7s", true, 1)]
        [TestCase("2s 2c 3s 4s 5s 7s 7d", true, 2)]
        [TestCase("2s 2c 2h 4s 5s 7s 7d", true, 1, Description = "Three of a kind should not be detected as a pair.")]
        public void EvaluateOfAKind_DetectsPairWith7Cards(string handString, bool isPairFound, int expectedGroupsDiscovered)
        {
            var hand = new PokerHand(handString);

            HandResult result = hand.EvaluateOfAKind(2);

            Assert.That(result.IsResultFound, Is.EqualTo(isPairFound));
            Assert.That(result.ListOfDiscoveredHands.Count(), Is.EqualTo(expectedGroupsDiscovered));
        }

        [Test]
        [TestCase("2s 2c 3h 3s 4s", false, 0)]
        [TestCase("2s 2c 2h 4s 5s", true, 1)]
        [TestCase("2s 2c 2h 4s 5s 6s 7s", true, 1)]
        [TestCase("2s 2c 2h 4s 4d 4h 7s", true, 2)]
        public void EvaluateOfAKind_DetectsThreeOfAKind(string handString, bool isPairFound, int expectedGroupsDiscovered)
        {
            var hand = new PokerHand(handString);

            HandResult result = hand.EvaluateOfAKind(3);

            Assert.That(result.IsResultFound, Is.EqualTo(isPairFound));
            Assert.That(result.ListOfDiscoveredHands.Count(), Is.EqualTo(expectedGroupsDiscovered));
        }

        [Test]
        [TestCase("2s 2c 3h 3s 4s", false, 0)]
        [TestCase("2s 2c 2h 2d 5s", true, 1)]
        [TestCase("2s 2c 2h 2d 5s 6s 7s", true, 1)]
        public void EvaluateOfAKind_DetectsFourOfAKind(string handString, bool isPairFound, int expectedGroupsDiscovered)
        {
            var hand = new PokerHand(handString);

            HandResult result = hand.EvaluateOfAKind(4);

            Assert.That(result.IsResultFound, Is.EqualTo(isPairFound));
            Assert.That(result.ListOfDiscoveredHands.Count(), Is.EqualTo(expectedGroupsDiscovered));
        }

        [Test]
        [TestCase("2s 2c 3h 3s 4s", false, 0, Description = "Junk")]
        [TestCase("2s 3s 4s 5s 6s", true, 1, Description = "5 Card Straight Flush")]
        [TestCase("2s 3s 4s 5s 6s 7s 8s", true, 3, Description = "7 Card Straight Flush")]
        public void EvaluateForStraightFlush(string handString, bool isPairFound, int expectedDiscoveredHandCount)
        {
            var hand = new PokerHand(handString);

            HandResult result = hand.EvaluateForStraightFlush();

            Assert.That(result.IsResultFound, Is.EqualTo(isPairFound));
            Assert.That(result.ListOfDiscoveredHands.Count(), Is.EqualTo(expectedDiscoveredHandCount));
        }

        [Test]
        [TestCase("2s 2c 3h 3s 4s", false, 0, Description = "Junk")]
        [TestCase("2s 3s 4s 5s 6s", false, 0, Description = "Only straight flush")]
        [TestCase("Ts Js Qs Ks As", true, 1, Description = "Royal Flush")]
        [TestCase("2h Ts Js Qs Ks As 8s", true, 1, Description = "7 Card Containing Royal Flush")]
        public void EvaluateForRoyalFlush(string handString, bool isPairFound, int expectedDiscoveredHandCount)
        {
            var hand = new PokerHand(handString);

            HandResult result = hand.EvaluateForRoyalFlush();

            Assert.That(result.IsResultFound, Is.EqualTo(isPairFound));
            Assert.That(result.ListOfDiscoveredHands.Count(), Is.EqualTo(expectedDiscoveredHandCount));
        }

        

    }
}
