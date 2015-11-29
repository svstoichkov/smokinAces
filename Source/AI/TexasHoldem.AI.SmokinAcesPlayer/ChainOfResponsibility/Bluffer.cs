namespace TexasHoldem.AI.SmokinAcesPlayer.ChainOfResponsibility
{
    using Logic.Players;

    public class Bluffer : DecisionTaker
    {
        public override PlayerAction ProcessRequest(GetTurnContext context, double handValue, int raiseAmount)
        {
            var raiseCount = SmokinAcesPlayer.actions.Count;

            //if (raiseCount <= 1 && context.RoundType == GameRoundType.Turn)
            //{
            //    return PlayerAction.Raise(200);
            //}

            return this.Successor.ProcessRequest(context, handValue, raiseAmount);
        }
    }
}
