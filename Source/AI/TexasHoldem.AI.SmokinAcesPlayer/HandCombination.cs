namespace TexasHoldem.AI.SmokinAcesPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Logic;

    using TexasHoldem.Logic.Cards;

    public static class HandCombination
    {
        public static HandRankType GetBestHand(List<Card> hand)
        {
            var sortedHand = SortByRank(hand);

            if (IsStraightFlush(sortedHand))
            {
                return HandRankType.Flush;
            }
            if (IsFourOfAKind(sortedHand))
            {
                return HandRankType.FourOfAKind;
            }
            if (IsFullHouse(sortedHand))
            {
                return HandRankType.FullHouse;
            }
            if (IsFlush(sortedHand))
            {
                return HandRankType.Flush;
            }
            if (IsStraight(sortedHand))
            {
                return HandRankType.Straight;
            }
            if (IsThreeOfAKind(sortedHand))
            {
                return HandRankType.ThreeOfAKind;
            }
            if (IsTwoPair(sortedHand))
            {
                return HandRankType.TwoPairs;
            }
            if (IsOnePair(sortedHand))
            {
                return HandRankType.Pair;
            }

            return HandRankType.HighCard;
        }

        private static bool IsStraightFlush(List<Card> sortedHand)
        {
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

        private static bool IsFourOfAKind(List<Card> sortedHand)
        {
            for (var i = 0; i < sortedHand.Count() - 3; i++)
            {
                if (sortedHand[i].Type == sortedHand[i + 1].Type && sortedHand[i].Type == sortedHand[i + 2].Type && sortedHand[i].Type == sortedHand[i + 3].Type)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsFullHouse(List<Card> sortedHand)
        {
            var threeOfAKind = false;
            var pair = false;
            var threeOfAKindType = 0;

            for (var i = 0; i < sortedHand.Count() - 2; i++)
            {
                if (sortedHand[i].Type == sortedHand[i + 1].Type && sortedHand[i].Type == sortedHand[i + 2].Type)
                {
                    threeOfAKind = true;
                    threeOfAKindType = (int) sortedHand[i].Type;
                    break;
                }
            }

            for (var i = 0; i <= sortedHand.Count() - 2; i++)
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

        private static bool IsFlush(List<Card> sortedHand)
        {
            int diamondCount = 0, clubCount = 0, heartCount = 0, spadeCount = 0;
            for (var i = 0; i < sortedHand.Count(); i++)
            {
                switch (sortedHand[i].Suit)
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

        private static bool IsStraight(List<Card> sortedHand)
        {
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

        private static bool IsThreeOfAKind(List<Card> sortedHand)
        {
            for (var i = 0; i < sortedHand.Count() - 2; i++)
            {
                if (sortedHand[i].Type == sortedHand[i + 1].Type && sortedHand[i].Type == sortedHand[i + 2].Type)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsTwoPair(List<Card> sortedHand)
        {
            var pairCount = 0;
            for (var i = 0; i < sortedHand.Count() - 1; i++)
            {
                if (sortedHand[i].Type == sortedHand[i + 1].Type)
                {
                    pairCount++;
                    i++;
                }
            }
            if (pairCount >= 2)
            {
                return true;
            }

            return false;
        }

        private static bool IsOnePair(List<Card> sortedHand)
        {
            for (var i = 0; i < sortedHand.Count() - 1; i++)
            {
                if (sortedHand[i].Type == sortedHand[i + 1].Type)
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Card> SortByRank(List<Card> cards)
        {
            var newList = new List<Card>(cards);
            var random = new Random();

            if (newList.Count() <= 1)
            {
                return newList;
            }

            var pivot = newList[random.Next(newList.Count())];
            newList.Remove(pivot);

            var less = new List<Card>();
            var greater = new List<Card>();
            // Assign values to less or greater list
            foreach (var card in newList)
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