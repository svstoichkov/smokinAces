namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using AI.SmartPlayer;
    using AI.SmokinAcesPlayer;

    using Logic.Players;

    public class SmokinAcesVsSmart : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new SmokinAcesPlayer();
        private readonly IPlayer secondPlayer = new SmartPlayer();
        protected override IPlayer GetFirstPlayer()
        {
            return this.firstPlayer;
        }

        protected override IPlayer GetSecondPlayer()
        {
            return this.secondPlayer;
        }
    }
}