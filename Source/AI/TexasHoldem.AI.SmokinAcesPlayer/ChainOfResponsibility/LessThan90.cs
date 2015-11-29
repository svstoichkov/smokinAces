namespace TexasHoldem.AI.SmokinAcesPlayer.ChainOfResponsibility
{
    using Logic.Players;

    internal class LessThan90 : DecisionTaker
    {
        public override PlayerAction ProcessRequest(GetTurnContext context, double handValue, int raiseAmount)
        {
            if (handValue < 0.90)
            {
                var raiseCount = SmokinAcesPlayer.actions.Count;
                if (raiseCount <= 3)
                {
                    return PlayerAction.Raise(raiseAmount * 3);
                }
                
                return PlayerAction.CheckOrCall();
            }

            else if (this.Successor != null)
            {
                return this.Successor.ProcessRequest(context, handValue, raiseAmount);
            }

            return null;
        }
    }
}