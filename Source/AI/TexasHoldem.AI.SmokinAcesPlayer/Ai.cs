namespace TexasHoldem.AI.SmokinAcesPlayer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Extensions;
    
    public class Ai
    {
        public static double CalculateHandValue(List<Card> myHand, List<Card> communityCards)
        {
            double wins = 0;
            Parallel.For(0, Settings.GameSimulationsCount, i =>
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
            });

            return wins / Settings.GameSimulationsCount;
        }
    }
}