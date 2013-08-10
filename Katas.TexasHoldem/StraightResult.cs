using System.Collections.Generic;

namespace Katas.TexasHoldem
{
    public class StraightResult : HandResultBase
    {
        public StraightResult()
        {
        }

        public StraightResult(bool isResultFound, IEnumerable<PokerHand> listOfDiscoveredHands = null) : base(isResultFound, listOfDiscoveredHands)
        {
        }
    }
}