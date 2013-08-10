using System.Collections.Generic;

namespace Katas.TexasHoldem
{
    public class HandResult
    {
        private readonly List<CardSet> _listOfDiscoveredCardSets = new List<CardSet>();

        public HandResult()
        {
        }

        public HandResult(bool isResultFound, IEnumerable<PokerHand> listOfDiscoveredHands = null )
        {
            IsResultFound = isResultFound;
            
            if (listOfDiscoveredHands != null)
            {
                _listOfDiscoveredCardSets = new List<CardSet>(listOfDiscoveredHands);
            }
        }

        internal void AddDiscoveredHand(CardSet hand)
        {
            _listOfDiscoveredCardSets.Add(hand);
        }
        
        public bool IsResultFound { get; internal set; }
        public IReadOnlyList<CardSet> ListOfDiscoveredHands { get { return _listOfDiscoveredCardSets.AsReadOnly(); } }
    }
}