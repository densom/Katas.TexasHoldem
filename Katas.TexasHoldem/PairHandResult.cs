using System;

namespace Katas.TexasHoldem
{
    public class PairHandResult : HandResult
    {
        public override int HandRank()
        {
            return base.HandRank() + 1;
        }

        protected override int CompareHandOfSameRank(HandResult otherHand)
        {
            throw new NotImplementedException();
        }
    }
}