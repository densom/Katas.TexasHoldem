using NUnit.Framework;

namespace Katas.TexasHoldem.Tests
{
    [TestFixture]
    public class CardTests
    {
        [Test]
        [TestCase("4s", Faces.Spade)]
        [TestCase("4d", Faces.Diamond)]
        [TestCase("4c", Faces.Club)]
        [TestCase("4h", Faces.Heart)]
        public void CreateCard_ParseString_Faces(string cardString, Faces expectedFace)
        {
            var card = new Card(cardString);

            Assert.That(card.Face, Is.EqualTo(expectedFace));
        }

        [Test]
        [TestCase("4s", 4)]
        [TestCase("Ts", 10)]
        [TestCase("Js", 11)]
        [TestCase("Qs", 12)]
        [TestCase("Ks", 13)]
        [TestCase("As", 14)]
//        [TestCase("4h", 4)]
        public void CreateCard_ParseString_Values(string cardString, int expectedValue)
        {
            var card = new Card(cardString);

            Assert.That(card.Value, Is.EqualTo(expectedValue));
        }

        [Test]
        public void EqualTo_Succes()
        {
            var card = new Card("4s");
            var otherCard = new Card("4s");

            Assert.That(card, Is.EqualTo(otherCard));
        }
    }
}