namespace TexasHoldem.AI.SmokinAcesPlayer.ChainOfResponsibility
{
    using Logic;
    using Logic.Players;

    internal class LessThan100 : DecisionTaker
    {
        public override PlayerAction ProcessRequest(GetTurnContext context, double handValue, int raiseAmount)
        {
            if (context.RoundType >= GameRoundType.Turn)
            {
                return PlayerAction.Raise(context.MoneyLeft + 1);
            }

            var raiseCount = SmokinAcesPlayer.actions.Count;

            return PlayerAction.Raise(raiseAmount * 6 / (raiseCount + 1));
        }
    }
}