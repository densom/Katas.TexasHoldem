using System.Collections.Generic;

namespace Katas.TexasHoldem
{
    public class HandResultBase
    {
        private readonly List<PokerHand> _listOfDiscoveredHands = new List<PokerHand>();

        public HandResultBase(bool isResultFound, IEnumerable<PokerHand> listOfDiscoveredHands = null )
        {
            IsResultFound = isResultFound;
            
            if (listOfDiscoveredHands != null)
            {
                _listOfDiscoveredHands = new List<PokerHand>(listOfDiscoveredHands);
            }
        }
        
        public bool IsResultFound { get; protected set; }
        public IReadOnlyList<PokerHand> ListOfDiscoveredHands { get { return _listOfDiscoveredHands.AsReadOnly(); } }
    }
}