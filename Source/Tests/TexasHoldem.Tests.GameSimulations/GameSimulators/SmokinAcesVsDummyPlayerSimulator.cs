namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using AI.SmokinAcesPlayer;

    using TexasHoldem.AI.DummyPlayer;
    using TexasHoldem.Logic.Players;

    public class SmokinAcesVsDummyPlayerSimulator : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new SmokinAcesPlayer();
        private readonly IPlayer secondPlayer = new DummyPlayer();

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
