namespace TexasHoldem.AI.SmokinAcesPlayer.ChainOfResponsibility
{
    using System.Linq;

    using Logic.Players;

    using TexasHoldem.Logic;

    internal class LessThan80 : DecisionTaker
    {
        public override PlayerAction ProcessRequest(GetTurnContext context, double handValue, int raiseAmount)
        {
            if (handValue < 0.80)
            {
                if (context.MoneyToCall - context.MyMoneyInTheRound > raiseAmount * 2 && SmokinAcesPlayer.actions
                    .Any(x => !x.PlayerName.ToLower().Contains("bullet") && !x.PlayerName.ToLower().Contains("dadummest") && !x.PlayerName.ToLower().Contains("smart")))
                {
                    return PlayerAction.Fold();
                }

                var raiseCount = SmokinAcesPlayer.actions.Count;
                if (raiseCount <= 2 && context.RoundType > GameRoundType.Turn)
                {
                    return PlayerAction.Raise(raiseAmount * 2);
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
