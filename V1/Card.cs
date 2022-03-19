namespace CardGameSimulator.V1
{
    public enum Suit { Hearts, Diamonds, Clubs, Spades };
    public enum Rank { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King };
    public record Card(Rank Rank, Suit Suit);

    public static class Cards
    {
        public static List<Card> CreateDeck()
        {
            var deck = new List<Card>(52);
            for (var suit = 0; suit < 4; suit++)
            {
                for (var rank = 0; rank < 13; rank++)
                {
                    deck.Add(new Card((Rank)rank, (Suit)suit));
                }
            }
            return deck;
        }
    }

    public static class Extensions
    {
        public static List<Card> Shuffle(this List<Card> deck)
        {
            for (var i = deck.Count - 1; i >= 1; i--)
            {
                var j = Random.Shared.Next(0, i + 1);
                (deck[i], deck[j]) = (deck[j], deck[i]);
            }

            return deck;
        }

        // Returns -1 if card < other, 1 if card > other
        public static int Compare(this Card card, Card other, bool acesAreHigh)
        {
            if (card.Rank == other.Rank) return 0;

            if (acesAreHigh && (card.Rank == Rank.Ace || other.Rank == Rank.Ace))
            {
                return other.Rank == Rank.Ace ? -1 : 1;
            }
            else
            {
                return card.Rank < other.Rank ? -1 : 1;
            }
        }
    }
}
