using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameSimulator.V1
{
    public interface IStrategy
    {
        (Card, Guess) NextGuess(Game game);
    }

    public enum Guess
    {
        Higher,
        Lower,
        Same,
    }

    public class Game
    {
        private readonly IStrategy _strategy;
        private readonly Queue<Card> _deck;

        public bool Debug { get; set; }

        public List<Card> Hand { get; }
        public bool? AcesAreHigh { get; private set; }

        public Game(IStrategy strategy, bool debug)
        {
            _strategy = strategy;
            _deck = new Queue<Card>(Cards.CreateDeck().Shuffle());

            Debug = debug;

            Hand = new List<Card>(9);
            for (var i = 0; i < 9; i++)
            {
                Hand.Add(_deck.Dequeue());
            }
        }

        public void Step()
        {
            // Get the guess from the strategy.
            var (card, guess) = _strategy.NextGuess(this);

            // Flip the card.
            var next = _deck.Dequeue();

            // Compare the guess.
            var cmp = next.Compare(card, AcesAreHigh ?? false);
            var correct =
                (cmp < 0 && guess == Guess.Lower) ||
                (cmp > 0 && guess == Guess.Higher) ||
                (cmp == 0 && guess == Guess.Same);

            // Handle aces
            if (AcesAreHigh == null && card.Rank == Rank.Ace)
            {
                throw new InvalidOperationException("can't guess an aces yet");
            }

            if (AcesAreHigh == null && next.Rank == Rank.Ace)
            {
                correct = true;
                if (guess == Guess.Lower) AcesAreHigh = false;
                if (guess == Guess.Higher) AcesAreHigh = true;

                if (guess == Guess.Same) correct = false;
            }

            if (Debug)
            {
                Console.WriteLine($"Hand: {string.Join(',', Hand.Select(h => h.Rank))}  Guessed: {card.Rank} {guess} Flipped: {next.Rank}  Correct={correct}  ({_deck.Count})");
            }

            // Remove the guessed card, and add the new one back if needed
            Hand.Remove(card);
            if (correct)
            {
                Hand.Add(next);
            }
        }

        public int Play()
        {
            try
            {
                while (_deck.Count != 0 && Hand.Count != 0)
                {
                    Step();
                }

                return _deck.Count;
            }
            catch (Exception)
            {
                return _deck.Count;
            }            
        }
    }
}
