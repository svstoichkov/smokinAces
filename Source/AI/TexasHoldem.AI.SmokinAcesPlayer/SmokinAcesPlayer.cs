namespace TexasHoldem.AI.SmokinAcesPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ChainOfResponsibility;

    using Logic;
    using Logic.Cards;
    using Logic.Players;

    public class SmokinAcesPlayer : BasePlayer
    {
        public override string Name { get; } = "SmokinAcesPlayer_" + Guid.NewGuid();

        public static double handValue;
        public static int raiseAmount;
        public static HashSet<PlayerActionAndName> actions = new HashSet<PlayerActionAndName>();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            foreach (var action in context.PreviousRoundActions.Where(x => !x.PlayerName.Contains("SmokinAces") && x.Action != PlayerAction.CheckOrCall()))
            {
                actions.Add(action);
            }

            handValue = HandEvaluator.CalculateHandValue(new List<Card> { this.FirstCard, this.SecondCard }, this.CommunityCards.ToList());
            raiseAmount = (int)(handValue * 70) / (5 - (int)context.RoundType) + context.SmallBlind;

            if (context.RoundType == GameRoundType.PreFlop)
            {
                handValue -= 0.10;
            }
            
            if (context.MoneyLeft <= 0)
            {
                return PlayerAction.CheckOrCall();
            }
            
            if (context.MoneyLeft < 100)
            {
                if (raiseAmount > 2)
                {
                    raiseAmount /= 2;
                }
            }

            var bluffer = new Bluffer();
            var lessThan50 = new LessThan50();
            var lessThan60 = new LessThan60();
            var lessThan70 = new LessThan70();
            var lessThan80 = new LessThan80();
            var lessThan90 = new LessThan90();
            var lessThan100 = new LessThan100();

            bluffer.SetSuccessor(lessThan50);
            lessThan50.SetSuccessor(lessThan60);
            lessThan60.SetSuccessor(lessThan70);
            lessThan70.SetSuccessor(lessThan80);
            lessThan80.SetSuccessor(lessThan90);
            lessThan90.SetSuccessor(lessThan100);

            return bluffer.ProcessRequest(context, handValue, raiseAmount);
        }

        public override void EndHand(EndHandContext context)
        {
            actions.Clear();
        }
    }
}

// Bluffasaurus -> SmartPlayer
// Bullets -> BulletsPlayer
// ColdCall -> TodorAllIn
// hashTagNimoa -> DaDummestPlayerEver
// qAliRaza -> SmartPlayer
// Sparta - SpartaPlayer
// ThreeOfAKind -> 3OfAKind
// Turing -> TuringPlayer