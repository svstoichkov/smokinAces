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

            var handValue = HandEvaluator.CalculateHandValue(myHand, communityCards);
            var raiseAmount = (int)(handValue * 70) / (4 - (int)context.RoundType);

            if (context.MoneyLeft == 0)
            {
                return PlayerAction.CheckOrCall();
            }

            if (context.MoneyLeft < 100)
            {
                if (raiseAmount > 2)
                {
                    raiseAmount /= 2;
                }
            }

            if (handValue < 0.20)
            {
                if (context.MoneyToCall > raiseAmount + 1)
                {
                    return PlayerAction.Fold();
                }

                return PlayerAction.CheckOrCall();
            }

            if (handValue < 0.30)
            {
                if (context.MoneyToCall > raiseAmount / 2)
                {
                    return PlayerAction.Fold();
                }

                return PlayerAction.CheckOrCall();
            }

            if (handValue < 0.40)
            {
                if (context.MoneyToCall > raiseAmount)
                {
                    return PlayerAction.Fold();
                }

                return PlayerAction.CheckOrCall();
            }

            if (handValue > 0.80)
            {
                return PlayerAction.Raise(context.MoneyLeft);
            }

            if (handValue > 0.60)
            {
                return PlayerAction.Raise(raiseAmount * 3);
            }

            if (context.MyMoneyInTheRound > raiseAmount * 3)
            {
                return PlayerAction.CheckOrCall();
            }

            return PlayerAction.Raise(raiseAmount);
        }
    }
}