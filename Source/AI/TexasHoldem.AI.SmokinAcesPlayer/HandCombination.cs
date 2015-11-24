using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holdem
{
    using TexasHoldem.AI.SmokinAcesPlayer;
    using TexasHoldem.Logic.Cards;

    /// <summary>
    /// the most enjoyable class to program, evaluate the best 5 cards out of 7 cards
    /// also determines information needed to compare hands
    /// all hands are sorted
    /// </summary>
    public static class HandCombination
    {
        //get best class without running isRoyalFlush, since straightflush covers the royal flush
        public static HandType getBestHandEfficiently(List<Card> hand)
        {
            if (isStraightFlush(hand))
                return HandType.Flush;
            if (isFourOfAKind(hand))
                return HandType.FourOfAKind;
            if (isFullHouse(hand))
                return HandType.FullHouse;
            if (isFlush(hand))
                return HandType.Flush;
            if (isStraight(hand))
                return HandType.Straight;
            if (isThreeOfAKind(hand))
                return HandType.ThreeOfAKind;
            if (isTwoPair(hand))
                return HandType.TwoPair;
            if (isOnePair(hand))
                return HandType.OnePair;

            return HandType.HighCard;
        }
        //look for royal flush, removing pair using recursion
        public static bool isRoyalFlush(Hand hand)
        {
            hand.sortByRank();
            Hand simplifiedhand1, simplifiedhand2; //to be set the same as hand - cards are removed from this hand to evaluate straights separately without the interference of pairs or three-of-a-kind
            for (int i = 0; i <= hand.Count() - 2; i++)
            {
                if (hand[i] == hand[i+1])
                {
                    simplifiedhand1 = new Hand(hand);
                    simplifiedhand1.Remove(i);
                    simplifiedhand2 = new Hand(hand);
                    simplifiedhand2.Remove(i + 1);
                    if (HandCombination.isRoyalFlush(simplifiedhand1))
                        return true;
                    if (HandCombination.isRoyalFlush(simplifiedhand2))
                        return true;
                }
            }
            int currentsuit = hand.getCard(0).getSuit();
            if (hand.getCard(0).getRank() == 14 && hand.getCard(1).getRank() == 13 && hand.getCard(2).getRank() == 12 && hand.getCard(3).getRank() == 11 && hand.getCard(4).getRank() == 10 && hand.getCard(1).getSuit() == currentsuit && hand.getCard(2).getSuit() == currentsuit && hand.getCard(3).getSuit() == currentsuit && hand.getCard(4).getSuit() == currentsuit)
                return true;
            else
                return false;
        }
        //use recursion to get rid of pairs, then evaluate straight flush
        public static bool isStraightFlush(Hand hand)
        {
            hand.sortByRank();
            Hand simplifiedhand1, simplifiedhand2; //to be set the same as hand - cards are removed from this hand to evaluate straights separately without the interference of pairs or three-of-a-kind
            for (int i = 0; i <= hand.Count() - 2; i++)
            {
                if (hand.getCard(i) == hand.getCard(i + 1))
                {
                    simplifiedhand1 = new Hand(hand);
                    simplifiedhand1.Remove(i);
                    simplifiedhand2 = new Hand(hand);
                    simplifiedhand2.Remove(i + 1);
                    if (HandCombination.isStraightFlush(simplifiedhand1))
                        return true;
                    if (HandCombination.isStraightFlush(simplifiedhand2))
                        return true;
                }
            }
            for (int i = 0; i <= hand.Count() - 5; i++)
            {
                int currentrank = hand.getCard(i).getRank(), currentsuit = hand.getCard(i).getSuit();
                if (currentrank == hand.getCard(i + 1).getRank() + 1 && currentrank == hand.getCard(i + 2).getRank() + 2 && currentrank == hand.getCard(i + 3).getRank() + 3 && currentrank == hand.getCard(i + 4).getRank() + 4 && currentsuit == hand.getCard(i + 1).getSuit() && currentsuit == hand.getCard(i + 2).getSuit() && currentsuit == hand.getCard(i + 3).getSuit() && currentsuit == hand.getCard(i + 4).getSuit())
                    return true;
                
            }
            for (int i = 0; i <= hand.Count() - 4; i++)
            {
                int currentrank = hand.getCard(i).getRank(), currentsuit = hand.getCard(i).getSuit();
                if (currentrank == 5 && hand.getCard(i + 1).getRank() == 4 && hand.getCard(i + 2).getRank() == 3 && hand.getCard(i + 3).getRank() == 2 && hand.getCard(0).getRank() == 14 && currentsuit == hand.getCard(i + 1).getSuit() && currentsuit == hand.getCard(i + 2).getSuit() && currentsuit == hand.getCard(i + 3).getSuit() && currentsuit == hand.getCard(0).getSuit())
                    return true;
            }
            return false;
        }

        //easy algorithm to understand, just loop through the array and check for a certain amount of pairs
        //same for 3 of a kind, full house, 2 pair and 1 pair
        public static bool isFourOfAKind(Hand hand)
        {
            hand.sortByRank();
            for (int i = 0; i <= hand.Count() - 4; i++)
            {
                if (hand.getCard(i) == hand.getCard(i + 1) && hand.getCard(i) == hand.getCard(i + 2) && hand.getCard(i) == hand.getCard(i + 3))
                    return true;
            }
            return false;
        }

        public static bool isFullHouse(Hand hand)
        {
            hand.sortByRank();
            bool threeofakind = false, pair = false;
            int threeofakindRank = 0;
            for (int i = 0; i <= hand.Count() - 3; i++)
            {
                if (hand.getCard(i) == hand.getCard(i + 1) && hand.getCard(i) == hand.getCard(i + 2))
                {
                    threeofakind = true;
                    threeofakindRank = hand.getCard(i).getRank();
                    break;
                }
            }
            for (int i = 0; i <= hand.Count() - 2; i++)
            {
                if (hand.getCard(i) == hand.getCard(i + 1) && hand.getCard(i).getRank() != threeofakindRank)
                {
                    pair = true;
                    break;
                }
            }
            if (threeofakind == true && pair == true)
                return true;
            else
                return false;
        }

        //use a counter, if a counter reaches five, a flush is detected
        public static bool isFlush(Hand hand)
        {
            hand.sortByRank();
            int diamondCount = 0, clubCount = 0, heartCount = 0, spadeCount = 0;
            for (int i = 0; i < hand.Count(); i++)
            {
                if ((SUIT)hand.getCard(i).getSuit() == SUIT.DIAMONDS)
                    diamondCount++;
                else if ((SUIT)hand.getCard(i).getSuit() == SUIT.CLUBS)
                    clubCount++;
                else if ((SUIT)hand.getCard(i).getSuit() == SUIT.HEARTS)
                    heartCount++;
                else if ((SUIT)hand.getCard(i).getSuit() == SUIT.SPADES)
                    spadeCount++;
            }
            if (diamondCount >= 5)
                return true;
            else if (clubCount >= 5)
                return true;
            else if (heartCount >= 5)
                return true;
            else if (spadeCount >= 5)
                return true;
            return false;
        }

        //explanation below
        public static bool isStraight(Hand hand)
        {
            hand.sortByRank();
            if(hand.getCard(0).getRank()==14)
                hand.Add(new Card((int)RANK.ACE,hand.getCard(0).getSuit()));
            int straightCount=1;
            for (int i = 0; i <= hand.Count() - 2; i++)
            {
                //if 5 cards are found to be straights, break out of the loop
                if (straightCount == 5)
                    break;
                int currentrank = hand.getCard(i).getRank();
                //if cards suit differ by 1, increment straight
                if (currentrank - hand.getCard(i + 1).getRank() == 1)
                    straightCount++;
                //specific condition for 2-A
                else if (currentrank == 2 && hand.getCard(i + 1).getRank() == 14)
                    straightCount++;
                //if cards suit differ by more than 1, reset straight to 1
                else if (currentrank - hand.getCard(i + 1).getRank() > 1)
                    straightCount = 1;
                //if card suits does not differ, do nothing
            }
            if (hand.getCard(0).getRank() == 14)
                hand.Remove(hand.Count() - 1);
            //depending on the straight count, return true or false
            if (straightCount == 5)
                return true;
            return false;
        }

        public static bool isThreeOfAKind(Hand hand)
        {
            hand.sortByRank();
            for (int i = 0; i <= hand.Count() - 3; i++)
            {
                if (hand.getCard(i) == hand.getCard(i + 1) && hand.getCard(i) == hand.getCard(i + 2))
                    return true;
            }
            return false;
        }
        
        public static bool isTwoPair(Hand hand)
        {
            hand.sortByRank();
            int pairCount = 0;
            for (int i = 0; i <= hand.Count() - 2; i++)
            {
                if (hand.getCard(i) == hand.getCard(i + 1))
                {
                    pairCount++;
                    i++; //the pair has already been checked, i must be incremented an additional time to avoid using a card in this pair again. This prevents the program from identifying 3 of a kind as 2 pairs.
                }
            }
            if (pairCount >= 2)
                return true;
            else
                return false;
        }
        public static bool isOnePair(Hand hand)
        {
            hand.sortByRank();
            for (int i = 0; i <= hand.Count() - 2; i++)
            {
                if (hand.getCard(i) == hand.getCard(i + 1))
                    return true;
            }
            return false;
        }

        private List<Card> SortByRank(List<Card> myCards)
        {
            var random = new Random();
            
            var pivot = myCards[random.Next(myCards.Count())];
            myCards.Remove(pivot);

            var less = new List<Card>();
            var greater = new List<Card>();
            // Assign values to less or greater list
            foreach (Card i in myCards)
            {
                if (i > pivot)
                {
                    greater.Add(i);
                }
                else if (i <= pivot)
                {
                    less.Add(i);
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
