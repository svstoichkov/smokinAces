namespace TexasHoldem.AI.SmokinAcesPlayer
{
    using System;
    using System.Collections.Generic;

    using Logic;
    using Logic.Cards;
    using Logic.Players;

    public class SmokinAcesPlayer : BasePlayer
    {
        private Random rand = new Random();

        public override string Name { get; } = "SmokinAcesPlayer_" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            var myHand = new List<Card> { this.FirstCard, this.SecondCard };
            var communityCards = new List<Card>(this.CommunityCards);

            var handValue = Ai.CalculateHandValue(myHand, communityCards);
            var raiseAmount = (int) (handValue * 70) / (4 - (int) context.RoundType);

            if (context.MoneyLeft < 100 && !context.IsAllIn && (int)context.RoundType > 0)
            {
                if (handValue < 0.20)
                {
                    return PlayerAction.Fold();
                }
            }

            if (handValue < 0.15)
            {
                if (context.MoneyToCall != 0)
                {
                    return PlayerAction.Fold();
                }

                return PlayerAction.CheckOrCall();
            }

            if (handValue < 0.25)
            {
                if (context.MoneyToCall <= raiseAmount / 3)
                {
                    return PlayerAction.CheckOrCall();
                }

                return PlayerAction.Fold();
            }

            if (handValue < 0.35)
            {
                if (context.MoneyToCall <= raiseAmount * 2 / 3)
                {
                    return PlayerAction.CheckOrCall();
                }

                return PlayerAction.Fold();
            }

            if (handValue > 0.70)
            {
                return PlayerAction.Raise(context.MoneyLeft + 100);
            }

            if (handValue > 0.55)
            {
                return PlayerAction.Raise(raiseAmount * 3);
            }

            return PlayerAction.Raise(raiseAmount);
        }
    }
}