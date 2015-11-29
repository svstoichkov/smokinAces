namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using AI.SmokinAcesPlayer;

    using Bullets.Logic;

    using Logic.Players;

    public class SmokinAcesVsBullet : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new SmokinAcesPlayer();
        private readonly IPlayer secondPlayer = new BulletsPlayer();

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