namespace CardGameSimulator.V2
{
    public static class ComparerExtensions
    {
        public static bool IsAce(this Card card) => card.Rank == Rank.Ace;

        public static bool IsSameRankAs(this Card card, Card other) => card.Rank == other.Rank;

        public static bool IsLowerRankThan(this Card card, Card other, bool acesAreHigh)
        {
            var x = card.Rank;
            var y = other.Rank;

            if (acesAreHigh)
            {
                if (x == Rank.Ace) x = Rank.Ace_High;
                if (y == Rank.Ace) y = Rank.Ace_High;
            }

            return x < y;
        }

        public static bool IsGreaterRankThan(this Card card, Card other, bool acesAreHigh)
        {
            var x = card.Rank;
            var y = other.Rank;

            if (acesAreHigh)
            {
                if (x == Rank.Ace) x = Rank.Ace_High;
                if (y == Rank.Ace) y = Rank.Ace_High;
            }

            return x > y;
        }
    }

    public class CardComparerAcesLow : IComparer<Card>
    {
        public static readonly IComparer<Card> Instance = new CardComparerAcesLow();

        public int Compare(Card x, Card y)
        {
            var diff = x.Rank - y.Rank;
            if (diff != 0) return diff;

            return x.Suit - y.Suit;
        }
    }

    public class CardComparerAcesHigh : IComparer<Card>
    {
        public static readonly IComparer<Card> Instance = new CardComparerAcesHigh();

        public int Compare(Card x, Card y)
        {
            var x1 = x.IsAce() ? Rank.Ace_High : x.Rank;
            var y1 = y.IsAce() ? Rank.Ace_High : y.Rank;

            var diff = x1 - y1;
            if (diff != 0) return diff;

            return x.Suit - y.Suit;
        }
    }
}
