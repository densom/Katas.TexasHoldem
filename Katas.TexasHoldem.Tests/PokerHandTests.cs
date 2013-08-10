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

        
    }
}
