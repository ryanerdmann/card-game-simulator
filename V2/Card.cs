namespace CardGameSimulator.V2
{
    public enum Suit : int { Hearts, Diamonds, Clubs, Spades };
    public enum Rank : int { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace_High };
    public record struct Card(Rank Rank, Suit Suit);
}
