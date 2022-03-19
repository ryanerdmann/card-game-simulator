using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameSimulator.V1
{
    public class RandomStrategy : IStrategy
    {
        public (Card, Guess) NextGuess(Game game)
        {
            while (true)
            {
                var (card, guess) = NextGuessImpl(game);

                if (game.AcesAreHigh == null && card.Rank == Rank.Ace)
                {
                    if (game.Hand.All(c => c.Rank == Rank.Ace))
                    {

                        throw new InvalidOperationException("All aces");
                    }
                    continue;
                }

                return (card, guess);
            }
        }

        public (Card, Guess) NextGuessImpl(Game game)
        {
            // Pick a card at random
            var idx = Random.Shared.Next(0, game.Hand.Count);
            var card = game.Hand[idx];

            var guess = (Random.Shared.Next() % 2 == 0)
                ? Guess.Lower : Guess.Higher;

            return (card, guess);
        }
    }
}
