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
            
            //Tuka shte napravim logikata s nqkolko if-a spored handValue kakvo da pravi
            return PlayerAction.CheckOrCall();
        }
    }
}
