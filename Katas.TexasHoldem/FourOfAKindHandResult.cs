using System;

namespace Katas.TexasHoldem
{
    public class FourOfAKindHandResult : HandResult
    {
        public override int HandRank()
        {
            return new ThreeOfAKindHandResult().HandRank() + 1;
        }

        protected override int CompareHandOfSameRank(HandResult otherHand)
        {
            throw new NotImplementedException();
        }
    }
}