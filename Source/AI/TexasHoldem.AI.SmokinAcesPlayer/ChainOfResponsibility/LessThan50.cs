namespace TexasHoldem.AI.SmokinAcesPlayer.ChainOfResponsibility
{
    using Logic.Players;

    internal class LessThan50 : DecisionTaker
    {
        public override PlayerAction ProcessRequest(GetTurnContext context, double handValue, int raiseAmount)
        {
            if (handValue < 0.50)
            {
                if (context.MoneyToCall > raiseAmount / 1.5)
                {
                    return PlayerAction.Fold();
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
