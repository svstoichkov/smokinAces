namespace TexasHoldem.AI.SmokinAcesPlayer.ChainOfResponsibility
{
    using System.Linq;

    using Logic.Players;

    internal class LessThan90 : DecisionTaker
    {
        public override PlayerAction ProcessRequest(GetTurnContext context, double handValue, int raiseAmount)
        {
            if (handValue < 0.90)
            {
                if (context.MoneyToCall - context.MyMoneyInTheRound > raiseAmount * 2 && SmokinAcesPlayer.actions
                    .Any(x => !x.PlayerName.ToLower().Contains("bullet") && !x.PlayerName.ToLower().Contains("dadummest") && !x.PlayerName.ToLower().Contains("smart")))
                {
                    return PlayerAction.Fold();
                }

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