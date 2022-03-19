using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameSimulator.V1
{
    public class CountingStrategy : IStrategy
    {
        private readonly List<Card> _remaining = Cards.CreateDeck();

        public (Card, Guess) NextGuess(Game game)
        {
            foreach (var card in game.Hand)
            {
                var same = _remaining.FirstOrDefault(c => c.Rank == card.Rank && c.Suit == card.Suit);
                if (same != null) _remaining.Remove(same);
            }

            // TODO: Cache the last flipped

            // For each card, count how many remaining are higher/lower/same.

            int best = -1;
            var guess = (game.Hand[0], Guess.Same);

            foreach (var card in game.Hand)
            {
                if (game.AcesAreHigh == null && card.Rank == Rank.Ace)
                {
                    // Can't use it yet
                    continue;
                }

                int higher = 0, lower = 0, same = 0;
                foreach (var remaining in _remaining)
                {
                    // This could go either way
                    if (game.AcesAreHigh == null && remaining.Rank == Rank.Ace)
                    {
                        lower++;
                        higher++;
                        continue;
                    }

                    var cmp = remaining.Compare(card, game.AcesAreHigh ?? false);

                    if (cmp == 0) same++;
                    if (cmp < 0) lower++;
                    if (cmp > 0) higher++;
                }

                int cardBest = Math.Max(same, Math.Max(lower, higher));

                if (cardBest > best)
                {
                    if (cardBest == same)
                    {
                        guess = (card, Guess.Same);
                    }
                    else if (cardBest == lower)
                    {
                        guess = (card, Guess.Lower);
                    }
                    else if (cardBest == higher)
                    {
                        guess = (card, Guess.Higher);
                    }

                    best = cardBest;
                }
            }

            return guess;
        }
    }
}
