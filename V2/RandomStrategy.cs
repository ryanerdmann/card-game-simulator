namespace CardGameSimulator.V2
{
    public class RandomStrategy : IStrategy
    {
        public void Initialize(Game game) { }

        public (Card chosen, GuessType guess) Guess(Game game)
        {
            while (true)
            {
                var idx = Random.Shared.Next(0, game.Hand.Count);
                var result = (game.Hand.ElementAt(idx), Coin.Flip(GuessType.Lower, GuessType.Higher));

                if (game.AcesAreHigh == null && result.Item1.IsAce())
                {
                    continue;
                }

                return result;
            }
        }
    }
}
