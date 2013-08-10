using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Katas.TexasHoldem
{
    public class FlushResult
    {
        private List<PokerHand> _listOfFlushHands = new List<PokerHand>();

        public bool IsFlushFound { get; private set; }
        public IReadOnlyList<PokerHand> ListOfFlushHands { get { return _listOfFlushHands.AsReadOnly(); } }

        public FlushResult(bool isFlushFound, IEnumerable<PokerHand> listOfFlushHands)
        {
            IsFlushFound = isFlushFound;
            _listOfFlushHands.AddRange(listOfFlushHands);
        }
    }
}