namespace TexasHoldem.AI.SmokinAcesPlayer.ChainOfResponsibility
{
    using Logic.Players;

    public abstract class DecisionTaker
    {
        protected DecisionTaker Successor { get; set; }

        public void SetSuccessor(DecisionTaker successor)
        {
            this.Successor = successor;
        }

        public abstract PlayerAction ProcessRequest(GetTurnContext context, double handValue, int raiseAmount);
    }
}