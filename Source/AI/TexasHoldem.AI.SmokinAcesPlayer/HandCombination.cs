namespace TexasHoldem.AI.SmokinAcesPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;

    public static class HandCombination
    {
        public static HandType GetBestHandEfficiently(List<Card> hand)
        {
            if (IsStraightFlush(hand))
            {
                return HandType.Flush;
            }
            if (IsFourOfAKind(hand))
            {
                return HandType.FourOfAKind;
            }
            if (IsFullHouse(hand))
            {
                return HandType.FullHouse;
            }
            if (IsFlush(hand))
            {
                return HandType.Flush;
            }
            if (IsStraight(hand))
            {
                return HandType.Straight;
            }
            if (IsThreeOfAKind(hand))
            {
                return HandType.ThreeOfAKind;
            }
            if (IsTwoPair(hand))
            {
                return HandType.TwoPair;
            }
            if (IsOnePair(hand))
            {
                return HandType.OnePair;
            }

            return HandType.HighCard;
        }

        private static bool IsStraightFlush(List<Card> hand)
        {
            var sortedHand = SortByRank(hand);

            for (var i = 0; i < sortedHand.Count - 4; i++)
            {
                if ((int) sortedHand[i].Type == (int) sortedHand[i + 1].Type - 1 &&
                    (int) sortedHand[i].Type == (int) sortedHand[i + 2].Type - 2 &&
                    (int) sortedHand[i].Type == (int) sortedHand[i + 3].Type - 3 &&
                    (int) sortedHand[i].Type == (int) sortedHand[i + 4].Type - 4 &&
                    sortedHand[i].Suit == sortedHand[i + 1].Suit &&
                    sortedHand[i].Suit == sortedHand[i + 2].Suit &&
                    sortedHand[i].Suit == sortedHand[i + 3].Suit &&
                    sortedHand[i].Suit == sortedHand[i + 4].Suit)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsFourOfAKind(List<Card> hand)
        {
            var sortedHand = SortByRank(hand);

            for (var i = 0; i < hand.Count() - 3; i++)
            {
                if (sortedHand[i].Type == sortedHand[i + 1].Type && sortedHand[i].Type == sortedHand[i + 2].Type && sortedHand[i].Type == sortedHand[i + 3].Type)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsFullHouse(List<Card> hand)
        {
            var sortedHand = SortByRank(hand);

            var threeOfAKind = false;
            var pair = false;
            var threeOfAKindType = 0;

            for (var i = 0; i < hand.Count() - 2; i++)
            {
                if (sortedHand[i].Type == sortedHand[i + 1].Type && sortedHand[i].Type == sortedHand[i + 2].Type)
                {
                    threeOfAKind = true;
                    threeOfAKindType = (int) sortedHand[i].Type;
                    break;
                }
            }

            for (var i = 0; i <= hand.Count() - 2; i++)
            {
                if (sortedHand[i].Type == sortedHand[i + 1].Type && (int) sortedHand[i].Type != threeOfAKindType)
                {
                    pair = true;
                    break;
                }
            }

            if (threeOfAKind && pair)
            {
                return true;
            }

            return false;
        }

        private static bool IsFlush(List<Card> hand)
        {
            int diamondCount = 0, clubCount = 0, heartCount = 0, spadeCount = 0;
            for (var i = 0; i < hand.Count(); i++)
            {
                switch (hand[i].Suit)
                {
                    case CardSuit.Diamond:
                        diamondCount++;
                        break;
                    case CardSuit.Club:
                        clubCount++;
                        break;
                    case CardSuit.Heart:
                        heartCount++;
                        break;
                    case CardSuit.Spade:
                        spadeCount++;
                        break;
                }
            }
            if (diamondCount > 4 || clubCount > 4 || heartCount > 4 || spadeCount > 4)
            {
                return true;
            }

            return false;
        }

        private static bool IsStraight(List<Card> hand)
        {
            var sortedHand = SortByRank(hand);

            for (var i = 0; i < sortedHand.Count - 4; i++)
            {
                if ((int) sortedHand[i].Type == (int) sortedHand[i + 1].Type - 1 &&
                    (int) sortedHand[i].Type == (int) sortedHand[i + 2].Type - 2 &&
                    (int) sortedHand[i].Type == (int) sortedHand[i + 3].Type - 3 &&
                    (int) sortedHand[i].Type == (int) sortedHand[i + 4].Type - 4)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsThreeOfAKind(List<Card> hand)
        {
            var sortedHand = SortByRank(hand);
            for (var i = 0; i < hand.Count() - 2; i++)
            {
                if (sortedHand[i].Type == sortedHand[i + 1].Type && sortedHand[i].Type == sortedHand[i + 2].Type)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsTwoPair(List<Card> hand)
        {
            var sortedHand = SortByRank(hand);
            var pairCount = 0;
            for (var i = 0; i < hand.Count() - 1; i++)
            {
                if (sortedHand[i].Type == sortedHand[i + 1].Type)
                {
                    pairCount++;

                    //the pair has already been checked, i must be incremented an additional time to avoid using a card in this pair again. 
                    //This prevents the program from identifying 3 of a kind as 2 pairs.
                    i++;
                }
            }
            if (pairCount >= 2)
            {
                return true;
            }

            return false;
        }

        private static bool IsOnePair(List<Card> hand)
        {
            var sortedHand = SortByRank(hand);
            for (var i = 0; i < hand.Count() - 1; i++)
            {
                if (sortedHand[i].Type == sortedHand[i + 1].Type)
                {
                    return true;
                }
            }
            return false;
        }

        private static List<Card> SortByRank(List<Card> cards)
        {
            var random = new Random();

            var pivot = cards[random.Next(cards.Count())];
            cards.Remove(pivot);

            var less = new List<Card>();
            var greater = new List<Card>();
            // Assign values to less or greater list
            foreach (var card in cards)
            {
                if (card.Type > pivot.Type)
                {
                    greater.Add(card);
                }
                else
                {
                    less.Add(card);
                }
            }

            // Recurse for less and greaterlists
            var list = new List<Card>();
            list.AddRange(SortByRank(greater));
            list.Add(pivot);
            list.AddRange(SortByRank(less));
            return list;
        }
    }
}