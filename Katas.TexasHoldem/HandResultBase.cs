using System.Collections.Generic;

namespace Katas.TexasHoldem
{
    public class HandResultBase
    {
        private readonly List<PokerHand> _listOfDiscoveredHands = new List<PokerHand>();

        public HandResultBase()
        {
        }

        public HandResultBase(bool isResultFound, IEnumerable<PokerHand> listOfDiscoveredHands = null )
        {
            IsResultFound = isResultFound;
            
            if (listOfDiscoveredHands != null)
            {
                _listOfDiscoveredHands = new List<PokerHand>(listOfDiscoveredHands);
            }
        }

        internal void AddDiscoveredHand(PokerHand hand)
        {
            _listOfDiscoveredHands.Add(hand);
        }
        
        public bool IsResultFound { get; internal set; }
        public IReadOnlyList<PokerHand> ListOfDiscoveredHands { get { return _listOfDiscoveredHands.AsReadOnly(); } }
    }
}