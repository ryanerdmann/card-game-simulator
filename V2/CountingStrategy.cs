namespace CardGameSimulator.V2
{
    public class CountingStrategy : IStrategy
    {
        private readonly List<Card> _remaining = new List<Card>(Deck.AllCards);

        public void Initialize(Game game)
        {
            foreach (var card in game.Hand)
            {
                _remaining.Remove(card);
            }
        }

        public (Card chosen, GuessType guess) Guess(Game game)
        {
            // Update our view of the remaining cards in the deck.
            _remaining.Remove(game.LastCard);

            // Find the card with the best chance.
            var best = -1;
            Card bestCard = default;
            GuessType bestGuess = default;

            foreach (var card in game.Hand)
            {
                // Can't use it yet.
                if (game.AcesAreHigh == null && card.IsAce()) continue;

                // Find the best guess for this card.
                FindBestGuess(game, card, ref best, ref bestCard, ref bestGuess);
            }

            return (bestCard, bestGuess);
        }

        private void FindBestGuess(Game game, Card card, ref int best, ref Card bestCard, ref GuessType bestGuess)
        {
            var acesAreWild = game.AcesAreHigh == null;
            var acesAreHigh = game.AcesAreHigh ?? false;

            int lower = 0, higher = 0, same = 0;

            foreach (var remaining in _remaining)
            {
                if (acesAreWild && remaining.IsAce())
                {
                    // Could count for either lower or higher.
                    lower++;
                    higher++;
                    continue;
                }

                if (remaining.IsLowerRankThan(card, acesAreHigh)) lower++;
                else if (remaining.IsGreaterRankThan(card, acesAreHigh)) higher++;
                else if (remaining.IsSameRankAs(card)) same++;
            }

            UpdateBestCard(ref best, ref bestCard, ref bestGuess, card, lower, higher, same);
        }

        private static void UpdateBestCard(ref int best, ref Card bestCard, ref GuessType bestGuess, Card card, int lower, int higher, int same)
        {
            var cardBest = Math.Max(lower, Math.Max(higher, same));

            if (cardBest > best || (cardBest == best && Coin.Flip()))
            {
                best = cardBest;
                bestCard = card;
                bestGuess = cardBest switch
                {
                    var x when x == higher && x == lower => Coin.Flip(GuessType.Higher, GuessType.Lower),
                    var x when x == higher => GuessType.Higher,
                    var x when x == lower => GuessType.Lower,
                    var x when x == same => GuessType.Same,
                    _ => throw new InvalidOperationException(),
                };
            }
        }
    }
}
