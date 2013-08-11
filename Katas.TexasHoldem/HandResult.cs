using System;
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

        public bool IsResultFound { get; internal set; }
        public IReadOnlyList<CardSet> ListOfDiscoveredHands { get { return _listOfDiscoveredCardSets.AsReadOnly(); } }

        internal void AddDiscoveredHand(CardSet hand)
        {
            _listOfDiscoveredCardSets.Add(hand);
        }


        public virtual int HandRank()
        {
            return 0;
        }

        protected virtual int CompareHandOfSameRank(HandResult otherHand)
        {
            throw new NotImplementedException();
        }

    }


}