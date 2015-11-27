namespace TexasHoldem.AI.SmokinAcesPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Logic;
    using Logic.Cards;
    using Logic.Players;

    public class SmokinAcesPlayer : BasePlayer
    {
        public override string Name { get; } = "SmokinAcesPlayer_" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            var handValue = HandEvaluator.CalculateHandValue(new List<Card> { this.FirstCard, this.SecondCard }, this.CommunityCards.ToList());
            var raiseAmount = (int)(handValue * 70) / (4 - (int)context.RoundType);

            if (context.RoundType == GameRoundType.PreFlop)
            {
                handValue -= 0.10;
            }

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

            if (handValue > 0.85)
            {
                return PlayerAction.Raise(context.MoneyLeft + 100);
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