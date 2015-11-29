namespace TexasHoldem.AI.SmokinAcesPlayer.ChainOfResponsibility
{
    using System.Linq;

    using Logic.Players;

    internal class LessThan60 : DecisionTaker
    {
        public override PlayerAction ProcessRequest(GetTurnContext context, double handValue, int raiseAmount)
        {
            if (handValue < 0.60)
            {
                var raiseCount = SmokinAcesPlayer.actions.Count;
                if (raiseCount == 0)
                {
                    return PlayerAction.Raise(raiseAmount);
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