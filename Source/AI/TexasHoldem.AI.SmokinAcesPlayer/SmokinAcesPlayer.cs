namespace TexasHoldem.AI.SmokinAcesPlayer
{
    using System;
    using System.Collections.Generic;

    using Logic.Cards;
    using Logic.Players;
    
    public class SmokinAcesPlayer : BasePlayer
    {
        public override string Name { get; } = "SmokinAcesPlayer_" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            var myHand = new List<Card> { this.FirstCard, this.SecondCard };
            var communityCards = new List<Card>(this.CommunityCards);

            var handValue = Ai.CalculateHandValue(myHand, communityCards);

            if (handValue < 0)
            {
                return PlayerAction.CheckOrCall();
            }

            if (handValue > 3000)
            {
                return PlayerAction.Raise(int.MaxValue);
            }

            if (handValue > 1000 && context.MoneyLeft > 10)
            {
                return PlayerAction.Raise(context.MoneyLeft / 10);
            }

            return PlayerAction.CheckOrCall();
        }
    }
}