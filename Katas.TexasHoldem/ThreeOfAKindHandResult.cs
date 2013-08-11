using System;

namespace Katas.TexasHoldem
{
    public class ThreeOfAKindHandResult : HandResult
    {
        public override int HandRank()
        {
            return new PairHandResult().HandRank() + 1;
        }

        protected override int CompareHandOfSameRank(HandResult otherHand)
        {
            throw new NotImplementedException();
        }
    }
}