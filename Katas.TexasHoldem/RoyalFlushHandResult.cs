using System.Collections.Generic;

namespace Katas.TexasHoldem
{
    public class RoyalFlushHandResult : HandResult
    {
        public RoyalFlushHandResult(bool isResultFound, IEnumerable<PokerHand> listOfDiscoveredHands = null) : base(isResultFound, listOfDiscoveredHands)
        {
        }

        protected override int BaseScore()
        {
            return 1000;
        }
    }
}