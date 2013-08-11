using System.Collections.Generic;

namespace Katas.TexasHoldem
{
    public class RoyalFlushHandResult : HandResult
    {
        public RoyalFlushHandResult(bool isResultFound, IEnumerable<PokerHand> listOfDiscoveredHands = null) : base(isResultFound, listOfDiscoveredHands)
        {
        }

        public override int HandRank()
        {
            return 1000;
        }
    }
}