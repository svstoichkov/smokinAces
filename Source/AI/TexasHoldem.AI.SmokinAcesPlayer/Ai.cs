namespace TexasHoldem.AI.SmokinAcesPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Extensions;
    
    public class Ai
    {
        public static int CalculateHandValue(List<Card> myHand, List<Card> communityCards)
        {
            var wins = 0;
            var loses = 0;

            for (var i = 0; i < Settings.GameSimulationsCount; i++)
            {
                var player1Hand = new List<Card>(myHand);
                var player2Hand = new List<Card>();

                var remainingCommunityCards = new List<Card>();

                var deck = new List<Card>(Deck.AllCards);
                deck.CustomRemoveRange(myHand);
                deck.CustomRemoveRange(communityCards);
                deck = deck.Shuffle().ToList();

                //generate remaining community cards
                for (int j = 0; j < 5 - communityCards.Count; j++)
                {
                    remainingCommunityCards.Add(deck.Deal());
                }

                //add hole/community cards to the AI
                player1Hand.AddRange(communityCards);
                player1Hand.AddRange(remainingCommunityCards);

                //add cards to player2
                player2Hand.Add(deck.Deal());
                player2Hand.Add(deck.Deal());
                player2Hand.AddRange(communityCards);
                player2Hand.AddRange(remainingCommunityCards);
                
                //compare hands
                var player1HandResult = HandCombination.GetBestHand(player1Hand);
                var player2HandResult = HandCombination.GetBestHand(player2Hand);

                if (player1HandResult > player2HandResult)
                {
                    wins++;
                }
                else if (player1HandResult < player2HandResult)
                {
                    loses++;
                }
            }

            return wins - loses;
        }

        //public void HardThinking(Pot mainPot)
        //{
        //    //if you lose the showdown, don't put any more money in the pot, check or fold
        //    if (this.handValue < 0.5)
        //    {
        //        if (this.getAmountToCall(mainPot) == 0)
        //        {
        //            this.Check(mainPot);
        //        }
        //        else
        //        {
        //            this.Fold(mainPot);
        //        }
        //        return;
        //    }
        //    //call/bet/raise if you win the showdown
        //    var firstPercent = 30;
        //    var random = this.rand.Next(100) + 1;
        //    //bet or all-in on the river
        //    if (this.myHand.Count() == 7)
        //    {
        //        if (random <= firstPercent)
        //        {
        //            if (this.getAmountToCall(mainPot) == 0)
        //            {
        //                this.Bet(this.BetAmount(mainPot), mainPot, index);
        //            }
        //            else
        //            {
        //                this.Raise(this.BetAmount(mainPot), mainPot, index);
        //            }
        //        }
        //        else
        //        {
        //            this.AllIn(mainPot, index);
        //        }
        //        return;
        //    }
        //    //call/bet before the river
        //    if (random <= firstPercent)
        //    {
        //        if (this.getAmountToCall(mainPot) == 0)
        //        {
        //            this.Check(mainPot);
        //        }
        //        else
        //        {
        //            this.Call(mainPot);
        //        }
        //    }
        //    else
        //    {
        //        if (this.getAmountToCall(mainPot) == 0)
        //        {
        //            this.Bet(this.BetAmount(mainPot), mainPot, index);
        //        }
        //        else
        //        {
        //            this.Raise(this.BetAmount(mainPot), mainPot, index);
        //        }
        //    }
        //}
        //
        ////how much to bet?
        //public int BetAmount(Pot mainPot)
        //{
        //    //bet 4 times the bigBlind before the flop
        //    if (this.myHand.Count() == 2)
        //    {
        //        return 4 * mainPot.BigBlind;
        //    }
        //    var rand = new Random();
        //    var randomValue = rand.Next(10);
        //    //under betting, between 1/2 - 3/4 of the pot
        //    if (randomValue < 2)
        //    {
        //        return rand.Next(mainPot.Amount / 4) + mainPot.Amount / 2;
        //    }
        //    //normal betting, between 3/4 to 1 of the pot
        //    if (randomValue < 8)
        //    {
        //        return rand.Next(mainPot.Amount / 4) + 3 * mainPot.Amount / 4;
        //    }
        //    //extreme bluffing, between 1 to 3 times the pot
        //    return rand.Next(mainPot.Amount * 2) + mainPot.Amount;
        //}
    }
}