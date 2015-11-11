namespace TexasHoldem.AI.SmokinAcesPlayer
{
    using System;

    //enums for difficulty and playingstyle of the AI
    //public enum DIFFICULTY
    //{
    //    EASY,
    //    MEDIUM,
    //    HARD
    //}
    //
    //public enum PLAYINGSTYLE
    //{
    //    BLUFFER,
    //    TIGHT,
    //    AGRESSIVE
    //}

    /// <summary>
    ///     class that support AI thinking for the easy, medium and hard level
    ///     have all functionality of the player class
    /// </summary>
    public class AIPlayer : Player
    {
        private readonly int difficulty;
        private readonly int playingStyle;
        private readonly Random rand;
        //construct name and information about AI based on playing style
        public AIPlayer()
        {
            this.difficulty = (int) DIFFICULTY.MEDIUM;
            this.playingStyle = (int) PLAYINGSTYLE.TIGHT;
            this.rand = new Random();
            this.HandValue = 0.5;
        }

        public AIPlayer(int buyInAmount, int difficulty, int playingStyle)
            : base(buyInAmount)
        {
            if (difficulty > 2 || playingStyle > 2)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.difficulty = difficulty;
            this.playingStyle = playingStyle;

            this.rand = new Random();
            this.HandValue = 0.5;
        }

        //public AIPlayer(int buyInAmount, DIFFICULTY difficulty, PLAYINGSTYLE playingStyle)
        //    : base(buyInAmount)
        //{
        //    this.difficulty = (int) difficulty;
        //    this.playingStyle = (int) playingStyle;
        //    if (playingStyle == PLAYINGSTYLE.BLUFFER)
        //    {
        //        this.Name = "Stewie";
        //        this.Information = "A champion bluffer, Stewie long ago learned the secret of making a fortune with nothing. " +
        //                           "He says, “What am I supposed to do, wait for the cards to go my way? I should live so long.” " +
        //                           "(A joke heard often around here goes, “How can you tell when Stewie is bluffing? His chips are moving!”)";
        //    }
        //    else if (playingStyle == PLAYINGSTYLE.AGRESSIVE)
        //    {
        //        this.Name = "Rachel";
        //        this.Information = "Don’t let her pretty face fool you. Rachel is what you might call an aggressive player. " +
        //                           "We’ve seen her beat six guys holding two clubs…and that was in the parking lot! " +
        //                           "(The graffiti in our poker room reads, “For a good time, don’t call Rachel. Just fold.”)";
        //    }
        //    else if (playingStyle == PLAYINGSTYLE.TIGHT)
        //    {
        //        this.Name = "Ray";
        //        this.Information = "A tight player, Ray studied for a career as an accountant, until he calculated it would be too risky. " +
        //                           "He knows the odds of everything, and plays by the numbers. If Ray is putting money in the pot, " +
        //                           "it’s because he expects it back…with interest of course.";
        //    }
        //    this.rand = new Random();
        //    this.HandValue = 0.5;
        //}

        //various propeties
        public double HandValue { get; private set; }

        //public double PotOdds { get; private set; }

        //public double RateOfReturn { get; private set; }

        //public string Information { get; }

        //make a decision about what to do
        public void MakeADecision(Pot mainPot, int index)
        {
            if (this.difficulty == (int) DIFFICULTY.EASY)
            {
                this.EasyThinking(mainPot, index);
            }
            else if (this.difficulty == (int) DIFFICULTY.MEDIUM)
            {
                this.MediumThinking(mainPot, index);
            }
            else
            {
                this.HardThinking(mainPot, index);
            }
        }

        //using the monte carlo method, calculate a hand value for the AI's current hand
        //this can be very hard on the computer's memory and processor and can result in lag time
        public void CalculateHandValue(int count)
        {
            double score = 0;
            PlayerList playerList = new PlayerList();
            for (var i = 0; i < count - 1; i++)
            {
                playerList.Add(new Player());
            }
            Hand bestHand = new Hand();
            var bestHandCount = 1;
            Deck deck = new Deck();
            Hand knownCommunityCards = new Hand();
            Hand remainingCommunityCards = new Hand();
            Hand myHoleCards = new Hand();
            //remove known cards from deck
            for (var i = 0; i < this.myHand.Count(); i++)
            {
                deck.Remove(this.myHand[i]);
            }
            //add known community cards
            for (var i = 2; i < this.myHand.Count(); i++)
            {
                knownCommunityCards.Add(this.myHand[i]);
            }
            myHoleCards.Add(this.getHand()[0]);
            myHoleCards.Add(this.getHand()[1]);
            //loop 100 times
            for (var i = 0; i < 100; i++)
            {
                //reset players and shuffle deck
                for (var j = 0; j < playerList.Count; j++)
                {
                    playerList[j].isbusted = false;
                    playerList[j].getHand().Clear();
                }
                this.myHand.Clear();
                remainingCommunityCards.Clear();
                deck.Shuffle();

                //generate remaining community cards
                if (knownCommunityCards.Count() < 5)
                {
                    remainingCommunityCards.Add(deck.Deal());
                    if (knownCommunityCards.Count() < 4)
                    {
                        remainingCommunityCards.Add(deck.Deal());
                        if (knownCommunityCards.Count() < 3)
                        {
                            remainingCommunityCards.Add(deck.Deal());
                            remainingCommunityCards.Add(deck.Deal());
                            remainingCommunityCards.Add(deck.Deal());
                        }
                    }
                }
                //add hole/community cards to the AI
                this.AddToHand(knownCommunityCards);
                this.AddToHand(remainingCommunityCards);
                this.AddToHand(myHoleCards);
                //add hole/community cards to other players
                for (var j = 0; j < playerList.Count; j++)
                {
                    playerList[j].AddToHand(knownCommunityCards);
                    if (remainingCommunityCards.Count() != 0)
                    {
                        playerList[j].AddToHand(remainingCommunityCards);
                    }
                    playerList[j].AddToHand(deck.Deal());
                    playerList[j].AddToHand(deck.Deal());
                    //if player is dealt hole cards of less than 5-5, and no pocket pairs the player drops out
                    if (playerList[j].getHand()[playerList[j].getHand().Count() - 1].getRank() + playerList[j].getHand()[playerList[j].getHand().Count() - 2].getRank() <= 10 && playerList[j].getHand()[playerList[j].getHand().Count() - 1].getRank() != playerList[j].getHand()[playerList[j].getHand().Count() - 2].getRank())
                    {
                        playerList[j].isbusted = true;
                    }
                }
                //add cards back to deck
                for (var j = 0; j < remainingCommunityCards.Count(); j++)
                {
                    deck.Add(remainingCommunityCards[j]);
                }
                for (var j = 0; j < playerList.Count; j++)
                {
                    deck.Add(playerList[j].getHand()[playerList[j].getHand().Count() - 1]);
                    deck.Add(playerList[j].getHand()[playerList[j].getHand().Count() - 2]);
                }
                //compare hands
                bestHandCount = 1;
                playerList.Add(this);
                bestHand = playerList[0].getHand();
                for (var j = 0; j < playerList.Count - 1; j++)
                {
                    if (playerList[j].isbusted)
                    {
                        continue;
                    }
                    if (HandCombination.getBestHandEfficiently(new Hand(playerList[j + 1].getHand())) > HandCombination.getBestHandEfficiently(new Hand(playerList[j].getHand())))
                    {
                        bestHandCount = 1;
                        bestHand = playerList[j + 1].getHand();
                    }
                    else if (HandCombination.getBestHandEfficiently(new Hand(playerList[j + 1].getHand())) == HandCombination.getBestHandEfficiently(new Hand(playerList[j].getHand())))
                    {
                        bestHandCount++;
                    }
                }
                playerList.Remove(this);
                //if my hand is the best, increment score
                if (this.myHand.isEqual(bestHand))
                {
                    score = score + 1 / bestHandCount;
                }
            }
            //reconstruct original hand
            this.myHand.Clear();
            this.AddToHand(myHoleCards);
            this.AddToHand(knownCommunityCards);
            //calculate hand value as a percentage of wins
            this.HandValue = score / 100;
        }

        //determine the hole cards of the player and the community cards to be dealt
        //determines who will win the showdown at the end
        public void CalculateHandValueHard(Hand otherHand, Deck deck)
        {
            Hand knownCommunityCards = new Hand();
            Hand remainingCommunityCards = new Hand();
            //add known community cards
            for (var i = 2; i < this.myHand.Count(); i++)
            {
                knownCommunityCards.Add(this.myHand[i]);
            }
            //generate remaining community cards
            if (knownCommunityCards.Count() < 5)
            {
                remainingCommunityCards.Add(deck.Deal());
                if (knownCommunityCards.Count() < 4)
                {
                    remainingCommunityCards.Add(deck.Deal());
                    if (knownCommunityCards.Count() < 3)
                    {
                        remainingCommunityCards.Add(deck.Deal());
                        remainingCommunityCards.Add(deck.Deal());
                        remainingCommunityCards.Add(deck.Deal());
                    }
                }
            }
            //add the remaining cards
            this.AddToHand(remainingCommunityCards);
            otherHand += remainingCommunityCards;
            otherHand += knownCommunityCards;

            //compare hands
            if (HandCombination.getBestHandEfficiently(new Hand(this.myHand)) > HandCombination.getBestHandEfficiently(new Hand(otherHand)))
            {
                this.HandValue = 1;
            }
            else if (HandCombination.getBestHandEfficiently(new Hand(this.myHand)) < HandCombination.getBestHandEfficiently(new Hand(otherHand)))
            {
                this.HandValue = 0;
            }
            else
            {
                this.HandValue = 0.5;
            }
            for (var i = 0; i < remainingCommunityCards.Count(); i++)
            {
                this.myHand.Remove(remainingCommunityCards[i]);
            }
        }

        //using random values decide on a move
        //public void EasyThinking(Pot mainPot, int index)
        //{
        //    int firstPercent = 0, secondPercent = 0, thirdPercent = 0;
        //
        //    //35% fold, 45% call, 18% raise, 2% all in
        //    if (this.playingStyle == (int) PLAYINGSTYLE.AGRESSIVE)
        //    {
        //        firstPercent = 35;
        //        secondPercent = 80;
        //        thirdPercent = 98;
        //    }
        //    //50% fold, 40% call, 9% raise, 1% all in
        //    else if (this.playingStyle == (int) PLAYINGSTYLE.TIGHT)
        //    {
        //        firstPercent = 50;
        //        secondPercent = 90;
        //        thirdPercent = 99;
        //    }
        //    //40% fold, 35% call, 22% raise, 3% all in
        //    else if (this.playingStyle == (int) PLAYINGSTYLE.BLUFFER)
        //    {
        //        firstPercent = 40;
        //        secondPercent = 75;
        //        thirdPercent = 97;
        //    }
        //    var random = this.rand.Next(100) + 1;
        //    if (random <= firstPercent)
        //    {
        //        if (this.getAmountToCall(mainPot) == 0)
        //        {
        //            this.Check(mainPot);
        //        }
        //        else
        //        {
        //            this.Fold(mainPot);
        //        }
        //    }
        //    else if (random > firstPercent && random <= secondPercent)
        //    {
        //        if (this.getAmountToCall(mainPot) == 0)
        //        {
        //            this.Check(mainPot);
        //        }
        //        else
        //        {
        //            this.Call(mainPot);
        //        }
        //    }
        //    else if (random > secondPercent && random <= thirdPercent)
        //    {
        //        if (this.getAmountToCall(mainPot) == 0)
        //        {
        //            this.Bet((this.rand.Next(15) + 10) * mainPot.MinimumRaise * 10 / 100, mainPot, index);
        //        }
        //        else
        //        {
        //            this.Raise((this.rand.Next(15) + 10) * mainPot.MinimumRaise * 10 / 100, mainPot, index);
        //        }
        //    }
        //    else if (random > thirdPercent)
        //    {
        //        this.AllIn(mainPot, index);
        //    }
        //}

        //using a rate of return value decide on a move
        //public void MediumThinking(Pot mainPot, int index)
        //{
        //    //automatically drop out if hand value is less than 5-5 and not pocket pairs
        //    if (this.myHand.Count() == 2)
        //    {
        //        if (this.myHand[0].getRank() + this.myHand[1].getRank() <= 10 && this.myHand[0].getRank() != this.myHand[1].getRank())
        //        {
        //            if (this.getAmountToCall(mainPot) == 0)
        //            {
        //                this.Check(mainPot);
        //            }
        //            else
        //            {
        //                this.Fold(mainPot);
        //            }
        //            return;
        //        }
        //    }
        //    var amountToCall = this.getAmountToCall(mainPot);
        //    //if nothing to call then hypothetically call 3/4 to 1 full pot
        //    if (amountToCall == 0)
        //    {
        //        if (this.myHand.Count() == 2)
        //        {
        //            amountToCall = mainPot.Amount;
        //        }
        //        else
        //        {
        //            amountToCall = 3 * mainPot.Amount / 4;
        //        }
        //    }
        //    //calculate potOdds amountToCall/(amountInPot+amountToCall)
        //    this.PotOdds = Convert.ToDouble(amountToCall) / Convert.ToDouble(amountToCall + mainPot.Amount);
        //    //rate of return is determined by dividing the two numbers
        //    this.RateOfReturn = this.HandValue / this.PotOdds;
        //    //using rate of return and hand value make appropiate action
        //    int firstPercent = 0, secondPercent = 0, thirdPercent = 0;
        //    double firstThreshold = 0, secondThreshold = 0, thirdThreshold = 0;
        //    if (amountToCall == 0)
        //    {
        //        firstThreshold = 0.65;
        //        secondThreshold = 0.88;
        //        thirdThreshold = 1.11;
        //    }
        //    else
        //    {
        //        firstThreshold = 1.00;
        //        secondThreshold = 1.60;
        //        thirdThreshold = 2.30;
        //    }
        //    if (this.playingStyle == (int) PLAYINGSTYLE.AGRESSIVE)
        //    {
        //        //95% fold, 0% call, 5% raise, 0% all in
        //        if (this.RateOfReturn < firstThreshold)
        //        {
        //            firstPercent = 95;
        //            secondPercent = 95;
        //            thirdPercent = 100;
        //        }
        //        //80% fold, 5% call, 15% raise, 0% all in
        //        else if (this.RateOfReturn < secondThreshold)
        //        {
        //            firstPercent = 80;
        //            secondPercent = 85;
        //            thirdPercent = 100;
        //        }
        //        //0% fold, 60% call, 39% raise, 1% all in
        //        else if (this.RateOfReturn < thirdThreshold)
        //        {
        //            firstPercent = 0;
        //            secondPercent = 60;
        //            thirdPercent = 99;
        //        }
        //        //0% fold, 30% call, 68% raise, 2% all in
        //        else if (this.RateOfReturn >= thirdThreshold)
        //        {
        //            firstPercent = 0;
        //            secondPercent = 30;
        //            thirdPercent = 98;
        //        }
        //    }
        //    else if (this.playingStyle == (int) PLAYINGSTYLE.TIGHT)
        //    {
        //        //99% fold, 0% call, 1% raise, 0% all in
        //        if (this.RateOfReturn < firstThreshold)
        //        {
        //            firstPercent = 99;
        //            secondPercent = 99;
        //            thirdPercent = 100;
        //        }
        //        //80% fold, 15% call, 5% raise, 0% all in
        //        else if (this.RateOfReturn < secondThreshold)
        //        {
        //            firstPercent = 80;
        //            secondPercent = 95;
        //            thirdPercent = 100;
        //        }
        //        //10% fold, 70% call, 20% raise, 0% all in
        //        else if (this.RateOfReturn < thirdThreshold)
        //        {
        //            firstPercent = 10;
        //            secondPercent = 80;
        //            thirdPercent = 100;
        //        }
        //        //0% fold, 40% call, 59% raise, 1% all in
        //        else if (this.RateOfReturn >= thirdThreshold)
        //        {
        //            firstPercent = 0;
        //            secondPercent = 40;
        //            thirdPercent = 99;
        //        }
        //    }
        //    else if (this.playingStyle == (int) PLAYINGSTYLE.BLUFFER)
        //    {
        //        //90% fold, 0% call, 10% raise, 0% all in
        //        if (this.RateOfReturn < firstThreshold)
        //        {
        //            firstPercent = 90;
        //            secondPercent = 90;
        //            thirdPercent = 100;
        //        }
        //        //70% fold, 10% call, 19% raise, 1% all in
        //        else if (this.RateOfReturn < secondThreshold)
        //        {
        //            firstPercent = 70;
        //            secondPercent = 80;
        //            thirdPercent = 99;
        //        }
        //        //0% fold, 50% call, 48% raise, 2% all in
        //        else if (this.RateOfReturn < thirdThreshold)
        //        {
        //            firstPercent = 0;
        //            secondPercent = 50;
        //            thirdPercent = 98;
        //        }
        //        //0% fold, 25% call, 70% raise, 5% all in
        //        else if (this.RateOfReturn >= thirdThreshold)
        //        {
        //            firstPercent = 0;
        //            secondPercent = 25;
        //            thirdPercent = 95;
        //        }
        //    }
        //    var random = this.rand.Next(100) + 1;
        //    if (random <= firstPercent)
        //    {
        //        if (this.getAmountToCall(mainPot) == 0)
        //        {
        //            this.Check(mainPot);
        //        }
        //        else
        //        {
        //            this.Fold(mainPot);
        //        }
        //    }
        //    else if (random > firstPercent && random <= secondPercent)
        //    {
        //        if (this.getAmountToCall(mainPot) == 0)
        //        {
        //            this.Bet(3 * mainPot.Amount / 4, mainPot, index);
        //        }
        //        else
        //        {
        //            this.Call(mainPot);
        //        }
        //    }
        //    else if (random > secondPercent && random <= thirdPercent)
        //    {
        //        if (this.getAmountToCall(mainPot) == 0)
        //        {
        //            this.Bet(this.BetAmount(mainPot), mainPot, index);
        //        }
        //        else
        //        {
        //            this.Raise(this.BetAmount(mainPot), mainPot, index);
        //        }
        //    }
        //    else if (random > thirdPercent)
        //    {
        //        this.AllIn(mainPot, index);
        //    }
        //}

        //using the knowledge of the opponent's cards, decide on a move
        public void HardThinking(Pot mainPot, int index)
        {
            //if you lose the showdown, don't put any more money in the pot, check or fold
            if (this.HandValue < 0.5)
            {
                if (this.getAmountToCall(mainPot) == 0)
                {
                    this.Check(mainPot);
                }
                else
                {
                    this.Fold(mainPot);
                }
                return;
            }
            //call/bet/raise if you win the showdown
            var firstPercent = 0;

            if (this.playingStyle == (int) PLAYINGSTYLE.AGRESSIVE)
            {
                firstPercent = 50;
            }
            else if (this.playingStyle == (int) PLAYINGSTYLE.TIGHT)
            {
                firstPercent = 30;
            }
            else if (this.playingStyle == (int) PLAYINGSTYLE.BLUFFER)
            {
                firstPercent = 70;
            }
            var random = this.rand.Next(100) + 1;
            //bet or all-in on the river
            if (this.myHand.Count() == 7)
            {
                if (random <= firstPercent)
                {
                    if (this.getAmountToCall(mainPot) == 0)
                    {
                        this.Bet(this.BetAmount(mainPot), mainPot, index);
                    }
                    else
                    {
                        this.Raise(this.BetAmount(mainPot), mainPot, index);
                    }
                }
                else
                {
                    this.AllIn(mainPot, index);
                }
                return;
            }
            //call/bet before the river
            if (random <= firstPercent)
            {
                if (this.getAmountToCall(mainPot) == 0)
                {
                    this.Check(mainPot);
                }
                else
                {
                    this.Call(mainPot);
                }
            }
            else
            {
                if (this.getAmountToCall(mainPot) == 0)
                {
                    this.Bet(this.BetAmount(mainPot), mainPot, index);
                }
                else
                {
                    this.Raise(this.BetAmount(mainPot), mainPot, index);
                }
            }
        }

        //how much to bet?
        public int BetAmount(Pot mainPot)
        {
            //bet 4 times the bigBlind before the flop
            if (this.myHand.Count() == 2)
            {
                return 4 * mainPot.BigBlind;
            }
            var rand = new Random();
            var randomValue = rand.Next(10);
            //under betting, between 1/2 - 3/4 of the pot
            if (randomValue < 2)
            {
                return rand.Next(mainPot.Amount / 4) + mainPot.Amount / 2;
            }
                //normal betting, between 3/4 to 1 of the pot
            if (randomValue < 8)
            {
                return rand.Next(mainPot.Amount / 4) + 3 * mainPot.Amount / 4;
            }
                //extreme bluffing, between 1 to 3 times the pot
            return rand.Next(mainPot.Amount * 2) + mainPot.Amount;
        }
    }
}