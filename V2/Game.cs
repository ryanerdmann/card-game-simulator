namespace CardGameSimulator.V2
{
    public interface IStrategy
    {
        void Initialize(Game game);
        (Card chosen, GuessType guess) Guess(Game game);
    }

    public enum GuessType { Higher, Lower, Same };

    public class Constants
    {
        public const int DECK_LENGTH = 52;
        public const int SUIT_COUNT = 4;
        public const int RANK_COUNT = 13;
        public const int HAND_SIZE = 9;
    }

    public class Game
    {
        private readonly IStrategy _strategy;
        private readonly Deck _deck;
        private readonly bool _debug;

        public bool? AcesAreHigh { get; private set; }
        public SortedSet<Card> Hand { get; private set; }
        public Card LastCard { get; private set; }

        public Game(IStrategy strategy, bool debug)
        {
            _strategy = strategy;
            _debug = debug;

            _deck = Deck.CreateShuffled();

            Hand = new SortedSet<Card>(CardComparerAcesLow.Instance);
            for (var i = 0; i < Constants.HAND_SIZE; i++)
            {
                Hand.Add(_deck.Next);
            }
        }

        public int Play()
        {
            _strategy.Initialize(this);

            while (_deck.Count > 0 && Hand.Count > 0 && !IsHandAllAces())
            {
                Guess();
            }

            return _deck.Count;
        }

        private bool IsHandAllAces() => Hand.Min.IsAce() && Hand.Max.IsAce();

        private void Guess()
        {
            // Get the user's guess.
            var (chosen, guess) = _strategy.Guess(this);

            // Make sure the guess is valid.
            if (AcesAreHigh == null && chosen.IsAce())
            {
                throw new InvalidOperationException("Canont guess Ace yet");
            }

            // Flip the next card.
            var next = _deck.Next;
            LastCard = next;

            // Check if the guess is correct.
            var correct = false;

            if (AcesAreHigh == null && next.IsAce())
            {
                correct = true;

                if (guess == GuessType.Lower) AcesAreHigh = false;
                if (guess == GuessType.Higher) AcesAreHigh = true;

                if (AcesAreHigh == true)
                {
                    Hand = new SortedSet<Card>(Hand, CardComparerAcesHigh.Instance);
                }
            }
            else
            {
                var acesAreHigh = AcesAreHigh ?? false;

                correct = guess switch
                {
                    GuessType.Same => next.IsSameRankAs(chosen),
                    GuessType.Lower => next.IsLowerRankThan(chosen, acesAreHigh),
                    GuessType.Higher => next.IsGreaterRankThan(chosen, acesAreHigh),
                    _ => throw new InvalidOperationException(),
                };
            }

            // Helpful debugging info.
            if (_debug)
            {
                Console.WriteLine($"Hand=[{string.Join(", ", Hand.Select(c => c.Rank))}] Guess={guess} {chosen.Rank} Flipped={next.Rank} Correct={correct} ({_deck.Count})");
            }

            // Update the game state.
            Hand.Remove(chosen);
            if (correct)
            {
                Hand.Add(next);
            }
        }
    }
}
