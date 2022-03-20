namespace CardGameSimulator.V2
{
    public enum StrategyType { Random, Basic, Counting, Hybrid };

    public static class StrategyFactory
    {
        public static IStrategy Create(StrategyType type) => type switch
        {
            StrategyType.Random => new RandomStrategy(),
            StrategyType.Basic => new BasicStrategy(),
            StrategyType.Counting => new CountingStrategy(),
            StrategyType.Hybrid => new HybridStrategy(new BasicStrategy(), new RandomStrategy(), 0.9),
            _ => throw new NotImplementedException(),
        };
    }
}
