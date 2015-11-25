namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using AI.DummyPlayer;
    using AI.SmokinAcesPlayer;

    using Logic.Players;

    public class SmokinAcesVsAlwaysCall : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new SmokinAcesPlayer();
        private readonly IPlayer secondPlayer = new AlwaysCallDummyPlayer();

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
