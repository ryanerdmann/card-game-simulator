namespace CardGameSimulator.V2
{
    public class BasicStrategy : IStrategy
    {
        public void Initialize(Game game) { }

        public (Card chosen, GuessType guess) Guess(Game game)
        {
            // The hand is sorted by Rank.
            var first = new CardInfo(GetFirst(game), game.AcesAreHigh);
            var last = new CardInfo(GetLast(game), game.AcesAreHigh);

            // Find the most extreme card in the hand, and choose it.
            var extreme =
                first.Distance > last.Distance ? first :
                last.Distance > first.Distance ? last :
                Coin.Flip(first, last);

            return (extreme.Card, extreme.Guess);
        }

        private static Card GetFirst(Game game) => game.Hand.First(c => IsValidCard(game, c));
        private static Card GetLast(Game game) => game.Hand.Last(c => IsValidCard(game, c));

        private static bool IsValidCard(Game game, in Card card)
        {
            return game.AcesAreHigh.HasValue || (game.AcesAreHigh == null && !card.IsAce());
        }

        // A234567_89TJQKA
        // A23456_7_89TJQK
        // 234567_8_9TJQKA

        private readonly struct CardInfo
        {
            private readonly int _diff;

            public CardInfo(Card card, bool? acesAreHigh)
            {
                Card = card;
                _diff = GetDiff(Card, acesAreHigh);
            }

            public Card Card { get; }

            public int Distance => Math.Abs(_diff);

            public GuessType Guess => _diff switch
            {
                > 0 => GuessType.Lower,
                < 0 => GuessType.Higher,
                _   => Coin.Flip(GuessType.Higher, GuessType.Lower),
            };

            private static int GetDiff(Card card, bool? acesAreHigh)
            {
                if (acesAreHigh.HasValue)
                {
                    var rank = (card.IsAce() && acesAreHigh == true)
                        ? Rank.Ace_High
                        : card.Rank;

                    var midpoint = acesAreHigh == true
                        ? Rank.Eight
                        : Rank.Seven;

                    return rank - midpoint;
                }
                else
                {
                    // If we do not know whether aces are low or high,
                    // treat 7.5 as the midpoint. The sign of the diff
                    // will still keep the Guess direction correct.
                    var rank = (int)card.Rank * 2;
                    var midpoint = ((int)Rank.Seven * 2) + 1;
                    return rank - midpoint;
                }
            }
        }
    }
}
