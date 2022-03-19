using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameSimulator.V1
{
    public class CardComparerAcesHigh : IComparer<Card>
    {
        public static IComparer<Card> Instance = new CardComparerAcesHigh();
        public int Compare(Card? x, Card? y)
        {
            var x1 = (int)x.Rank;
            var y1 = (int)y.Rank;

            if (x.Rank == Rank.Ace) x1 = 13;
            if (y.Rank == Rank.Ace) y1 = 13;

            return x1 - y1;
        }
    }

    public static class Coin
    {
        public static bool Flip()
        {
            return Random.Shared.Next() % 2 == 0;
        }

        public static T Flip<T>(T x, T y)
        {
            return Flip() ? x : y;
        }
    }

    public class CardComparerAcesLow : IComparer<Card>
    {
        public static IComparer<Card> Instance = new CardComparerAcesLow();
        public int Compare(Card? x, Card? y)
        {
            return x.Rank - y.Rank;
        }
    }

    public class BasicStrategy : IStrategy
    {
        // A23456_7_89TJQK
        // 234567_8_9TJQKA

        public (Card, Guess) NextGuess(Game game)
        {
            if (game.Hand.Count == 1)
            {
                var card = game.Hand.Single();

                if (game.AcesAreHigh == true)
                {
                    if (card.Rank == Rank.Eight) return (card, Coin.Flip(Guess.Higher, Guess.Lower));
                    if (card.Rank > Rank.Ace && card.Rank < Rank.Eight) return (card, Guess.Higher);
                    else return (card, Guess.Lower);
                }
                else
                {
                    if (card.Rank == Rank.Seven) return (card, Coin.Flip(Guess.Higher, Guess.Lower));
                    if (card.Rank < Rank.Seven) return (card, Guess.Higher);
                    else return (card, Guess.Lower);
                }
            }

            if (game.AcesAreHigh == true)
            {
                var ordered = game.Hand.Where(c =>
                {
                    if (game.AcesAreHigh.HasValue) return true;
                    return c.Rank != Rank.Ace;
                }).OrderBy(h => h, CardComparerAcesHigh.Instance).ToArray();

                var smallest = ordered.First();
                var largest = ordered.Last();

                var smallestRank = (int)smallest.Rank;
                var largestRank = (int)largest.Rank;

                if (smallest.Rank == Rank.Ace) smallestRank = 13;
                if (largest.Rank == Rank.Ace) largestRank = 13;

                var smallestDiff = Math.Abs(smallestRank - 7);
                var largestDiff = Math.Abs(largestRank - 7);

                if (smallestDiff == 0 && largestDiff == 0)
                {
                    return (Coin.Flip(largest, smallest), Coin.Flip(Guess.Lower, Guess.Higher));
                }
                else if (smallestDiff == largestDiff)
                {
                    if (Coin.Flip())
                    {
                        return (smallest, Guess.Higher);
                    }
                    else
                    {
                        return (largest, Guess.Lower);
                    }
                }
                else if (smallestDiff > largestDiff)
                {
                    return (smallest, Guess.Higher);
                }
                else
                {
                    return (largest, Guess.Lower);
                }
            }
            else
            {
                var ordered = game.Hand.Where(c =>
                {
                    if (game.AcesAreHigh.HasValue) return true;
                    return c.Rank != Rank.Ace;
                }).OrderBy(h => h, CardComparerAcesLow.Instance).ToArray();

                var smallest = ordered.First();
                var largest = ordered.Last();

                var smallestDiff = Math.Abs((int)smallest.Rank - 6);
                var largestDiff = Math.Abs((int)largest.Rank - 6);

                if (smallestDiff == 0 && largestDiff == 0)
                {
                    return (Coin.Flip(largest, smallest), Coin.Flip(Guess.Lower, Guess.Higher));
                }
                else if (smallestDiff == largestDiff)
                {
                    if (Coin.Flip())
                    {
                        return (smallest, Guess.Higher);
                    }
                    else
                    {
                        return (largest, Guess.Lower);
                    }
                }
                else if (smallestDiff > largestDiff)
                {
                    return (smallest, Guess.Higher);
                }
                else
                {
                    return (largest, Guess.Lower);
                }
            }
        }
    }
}
