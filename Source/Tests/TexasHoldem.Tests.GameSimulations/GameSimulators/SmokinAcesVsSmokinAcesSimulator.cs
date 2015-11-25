namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using AI.SmokinAcesPlayer;

    using TexasHoldem.AI.SmartPlayer;
    using TexasHoldem.Logic.Players;

    public class SmokinAcesVsSmokinAcesSimulator : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new SmokinAcesPlayer();
        private readonly IPlayer secondPlayer = new SmokinAcesPlayer();

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
