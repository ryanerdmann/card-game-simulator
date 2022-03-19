using static CardGameSimulator.V2.Constants;

namespace CardGameSimulator.V2
{
    public class Deck
    {
        private readonly Card[] _cards;
        private int _index = 0;

        public Deck(Card[] cards)
        {
            if (cards.Length != DECK_LENGTH)
            {
                throw new ArgumentException(nameof(cards));
            }

            _cards = cards;
        }

        public static readonly Card[] AllCards = CreateSorted()._cards;

        public int Count => DECK_LENGTH - _index;
        public Card Next => _cards[_index++];

        private void Shuffle()
        {
            for (var i = _cards.Length - 1; i >= 1; i--)
            {
                var j = Random.Shared.Next(0, i + 1);
                (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
            }
        }

        public static Deck CreateSorted()
        {
            var cards = new Card[DECK_LENGTH];
            var i = 0;

            for (var suit = 0; suit < SUIT_COUNT; suit++)
            {
                for (var rank = 0; rank < RANK_COUNT; rank++)
                {
                    cards[i++] = new Card((Rank)rank, (Suit)suit);
                }
            }

            return new Deck(cards);
        }

        public static Deck CreateShuffled()
        {
            var deck = CreateSorted();
            deck.Shuffle();
            return deck;
        }
    }
}
