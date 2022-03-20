namespace CardGameSimulator.V1
{
    public enum StrategyType { Random, Basic, Counting };

    public static class StrategyFactory
    {
        public static IStrategy Create(StrategyType type) => type switch
        {
            StrategyType.Random => new RandomStrategy(),
            StrategyType.Basic => new BasicStrategy(),
            StrategyType.Counting => new CountingStrategy(),
            _ => throw new NotImplementedException(),
        };
    }
}
