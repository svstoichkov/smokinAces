namespace TexasHoldem.AI.SmokinAcesPlayer.ChainOfResponsibility
{
    using System.Linq;

    using Logic.Players;

    internal class LessThan70 : DecisionTaker
    {
        public override PlayerAction ProcessRequest(GetTurnContext context, double handValue, int raiseAmount)
        {
            if (handValue < 0.70)
            {
                //if (context.MoneyToCall - context.MyMoneyInTheRound > raiseAmount * 2)
                //{
                //    return PlayerAction.Fold();
                //}

                var raiseCount = SmokinAcesPlayer.actions.Count;
                if (raiseCount <= 1)
                {
                    return PlayerAction.Raise((int) (raiseAmount * 1.5));
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