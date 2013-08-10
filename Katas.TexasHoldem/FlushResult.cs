using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Katas.TexasHoldem
{
    public class FlushResult : HandResultBase
    {
        public FlushResult(bool isResultFound, IEnumerable<PokerHand> listOfDiscoveredHands = null) : base(isResultFound, listOfDiscoveredHands)
        {
        }
    }
}