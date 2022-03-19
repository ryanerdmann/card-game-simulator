namespace CardGameSimulator.V2
{
    public class BasicStrategy : IStrategy
    {
        public void Initialize(Game game) { }

        public (Card chosen, GuessType guess) Guess(Game game)
        {
            var acesAreHigh = game.AcesAreHigh ?? false;

            // Find the most extreme card in the hand, and choose it.
            var smallest = new CardInfo(GetSmallest(game), acesAreHigh);
            var largest = new CardInfo(GetLargest(game), acesAreHigh);

            var extreme =
                smallest.Distance > largest.Distance ? smallest :
                largest.Distance > smallest.Distance ? largest :
                Coin.Flip(smallest, largest);

            return (extreme.Card, extreme.Guess);
        }

        private static Card GetSmallest(Game game)
        {
            for (var i = 0; i < game.Hand.Count; i++)
            {
                var card = game.Hand.ElementAt(i);
                if (IsValidCard(game, card)) return card;
            }

            throw new InvalidOperationException();
        }

        private static Card GetLargest(Game game)
        {
            for (var i = game.Hand.Count - 1; i >= 0; i--)
            {
                var card = game.Hand.ElementAt(i);
                if (IsValidCard(game, card)) return card;
            }

            throw new InvalidOperationException();
        }

        private static bool IsValidCard(Game game, in Card card)
        {
            return game.AcesAreHigh.HasValue || (game.AcesAreHigh == null && !card.IsAce());
        }

        // A23456_7_89TJQK
        // 234567_8_9TJQKA

        private readonly struct CardInfo
        {
            private readonly int _diff;

            public CardInfo(Card card, bool acesAreHigh)
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

            private static int GetDiff(Card card, bool acesAreHigh)
            {
                var rank = (card.IsAce() && acesAreHigh)
                    ? Rank.Ace_High
                    : card.Rank;

                var midpoint = acesAreHigh
                    ? Rank.Eight
                    : Rank.Seven;

                return rank - midpoint;
            }
        }
    }
}
