namespace CardGameSimulator.V2
{
    public class HybridStrategy : IStrategy
    {
        private readonly IStrategy _left;
        private readonly IStrategy _right;
        private readonly double _pLeft;

        public HybridStrategy(IStrategy left, IStrategy right, double pLeft)
        {
            _left = left;
            _right = right;
            _pLeft = pLeft;
        }

        public void Initialize(Game game) { }

        public (Card chosen, GuessType guess) Guess(Game game)
        {
            var strategy = (Random.Shared.NextDouble() <= _pLeft) ? _left : _right;
            return strategy.Guess(game);
        }
    }
}
