namespace CardGameSimulator.V2
{
    public static class Coin
    {
        public static bool Flip() => Random.Shared.NextDouble() > 0.5;
        public static T Flip<T>(T a, T b) => Flip() ? a : b;
    }
}
