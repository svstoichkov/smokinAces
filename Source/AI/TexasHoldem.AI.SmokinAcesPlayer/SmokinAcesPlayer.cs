namespace TexasHoldem.AI.SmokinAcesPlayer
{
    using System;

    using Logic.Players;

    public class SmokinAcesPlayer : BasePlayer
    {
        public override string Name { get; } = "SmokinAcesPlayer_" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
