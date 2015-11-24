namespace TexasHoldem.AI.SmokinAcesPlayer
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// this class includes the chipStack of the individual players and actions they can make
    /// in poker, a very important class
    /// </summary>
    public class Player : INotifyPropertyChanged
    {
        protected Hand myHand = new Hand();
        protected string name;
        protected int chipStack;
        protected int amountInPot;
        protected bool folded;
        protected int amountContributed;
        protected int initialStack;
        protected string message;
        protected string simplifiedMessage;
        public bool isbusted;
        //various propeties
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public int ChipStack
        {
            get { return this.chipStack; }
            set
            {
                if (value < 0)
                    value = 0;
                this.chipStack = value;
                this.InvokePropertyChanged(new PropertyChangedEventArgs("ChipStack"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) handler(this, e);
        }
        public int AmountContributed
        {
            get { return this.amountContributed; }
            set { this.amountContributed = value; }
        }
        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value;
            }
        }
        public string SimplifiedMessage
        {
            get
            {
                return this.simplifiedMessage;
            }
            set
            {
                this.simplifiedMessage = value;
                this.InvokePropertyChanged(new PropertyChangedEventArgs("SimplifiedMessage"));
            }
        }
        public int InitialStack
        {
            get { return this.initialStack; }
            set
            {
                if (value < 0)
                    value = 0;
                this.initialStack = value;
            }
        }
        public int AmountInPot
        {
            get { return this.amountInPot; }
            set
            {
                if (value < 0)
                    value = 0;
                this.amountInPot = value;
            }
        }
        public int getAmountToCall(Pot mainPot)
        {
            return mainPot.getMaximumAmountPutIn() - this.amountInPot;
        }
        //contruct player name and chipstack
        public Player()
        {
            this.name = "You";
            this.ChipStack = 10000;
            this.amountInPot = 0;
            this.initialStack = this.ChipStack;
            this.folded = false;
            this.message = "No decision has been made";
            this.simplifiedMessage = "";
            this.isbusted = false;
        }
        public Player(int buyInAmount)
        {
            this.ChipStack = buyInAmount;
            this.initialStack = this.ChipStack;
            this.amountInPot = 0;
            this.folded = false;
            this.message = "No decision has been made";
            this.simplifiedMessage = "";
            this.isbusted = false;
        }
        public Player(string name, int buyInAmount)
        {
            if (name == "")
                name = "You";
            this.name = name;
            this.ChipStack = buyInAmount;
            this.initialStack = this.ChipStack;
            this.amountInPot = 0;
            this.folded = false;
            this.message = "No decision has been made";
            this.simplifiedMessage = "";
            this.isbusted = false;
        }
        public Hand getHand()
        {
            return this.myHand;
        }
        //add hands to hand
        public void AddToHand(Hand hand)
        {
            this.myHand += hand;
        }
        public void AddToHand(Card card)
        {
            this.myHand.Add(card);
        }
        //pay the small and big blinds
        public void PaySmallBlind(int amount, Pot mainPot)
        {
            if (this.ChipStack <= amount)
            {
                this.AllIn(mainPot);
                return;
            }
            this.ChipStack -= amount;
            this.amountInPot += amount;
            mainPot.Add(amount);
            mainPot.AddPlayer(this);
            mainPot.setMaximumAmount(this.amountInPot);
            mainPot.MinimumRaise = amount;
            this.Message = this.Name + " pays the small blind";
            this.SimplifiedMessage = "SMALL BLIND " + amount;
        }
        //some functions have index as they are needed to reset the agressor index to continue the round
        public void PaySmallBlind(int amount, Pot mainPot,int index)
        {
            if (this.ChipStack <= amount)
            {
                this.AllIn(mainPot,index);
                return;
            }
            this.ChipStack -= amount;
            this.amountInPot += amount;
            mainPot.Add(amount);
            mainPot.AddPlayer(this);
            mainPot.setMaximumAmount(this.amountInPot);
            mainPot.MinimumRaise = amount;
            this.Message = this.Name + " pays the small blind";
            this.SimplifiedMessage = "SMALL BLIND " + amount;
        }
        public void PayBigBlind(int amount, Pot mainPot)
        {
            if (this.ChipStack <= amount)
            {
                this.AllIn(mainPot);
                return;
            }
            this.Message = "Pay blind of " + amount.ToString("c0");
            this.ChipStack -= amount;
            this.amountInPot += amount;
            mainPot.Add(amount);
            mainPot.AddPlayer(this);
            mainPot.setMaximumAmount(this.amountInPot);
            mainPot.MinimumRaise = amount;
            this.Message = this.Name + " pays the big blind";
            this.SimplifiedMessage = "BIG BLIND " + amount;
        }
        public void PayBigBlind(int amount, Pot mainPot, int index)
        {
            if (this.ChipStack <= amount)
            {
                this.AllIn(mainPot,index);
                return;
            }
            this.Message = "Pay blind of " + amount.ToString("c0");
            this.ChipStack -= amount;
            this.amountInPot += amount;
            mainPot.Add(amount);
            mainPot.AddPlayer(this);
            mainPot.setMaximumAmount(this.amountInPot);
            mainPot.MinimumRaise = amount;
            this.Message = this.Name + " pays the big blind";
            this.SimplifiedMessage = "BIG BLIND " + amount;
            mainPot.AgressorIndex = index;
        }
        //leave the round
        public void Fold(Pot mainPot)
        {

            this.folded = true;
            mainPot.getPlayersInPot().Remove(this);
            this.Message = "Fold";
            this.SimplifiedMessage = "FOLDED";

        }
        public bool IsFolded()
        {
            return this.folded;
        }
        //don't bet
        public void Check(Pot mainPot)
        {
            this.Message = "Check";
            this.SimplifiedMessage = "CHECK";
        }
        //bet enough to stay in the round
        public void Call(Pot mainPot)
        {
            
            int amount = mainPot.getMaximumAmountPutIn() - this.amountInPot;
            if (this.ChipStack <= amount)
            {
                this.AllIn(mainPot);
                return;
            }
            this.ChipStack -= amount;
            this.amountInPot += amount;
            mainPot.Add(amount);
            mainPot.AddPlayer(this);
            this.Message = "Call " + amount.ToString("c0");
            this.SimplifiedMessage = "CALL " + amount;
        }
        //call and bet additional amount of money
        public void Raise(int raise, Pot mainPot)
        {
            int amount = mainPot.getMaximumAmountPutIn() + raise - this.amountInPot;
            if (this.ChipStack <= amount)
            {
                this.AllIn(mainPot);
                return;
            }
            this.ChipStack -= amount;
            this.amountInPot += amount;
            mainPot.Add(amount);
            mainPot.setMaximumAmount(this.amountInPot);
            mainPot.AddPlayer(this);
            mainPot.MinimumRaise = raise;
            this.Message = "Call " + (amount - raise).ToString("c0") + " and raise " + raise.ToString("c0");
            this.SimplifiedMessage = "RAISE " + (amount - raise);
        }
        public void Raise(int raise, Pot mainPot, int index)
        {
            int amount = mainPot.getMaximumAmountPutIn() + raise - this.amountInPot;
            if (this.ChipStack <= amount)
            {
                this.AllIn(mainPot,index);
                return;
            }
            this.ChipStack -= amount;
            this.amountInPot += amount;
            mainPot.Add(amount);
            mainPot.setMaximumAmount(this.amountInPot);
            mainPot.AddPlayer(this);
            mainPot.MinimumRaise = raise;
            this.Message = "Call " + (amount - raise).ToString("c0") + " and raise " + raise.ToString("c0");
            this.SimplifiedMessage = "RAISE " + (amount - raise);
            mainPot.AgressorIndex = index;
        }
        //bet a certain amount of money
        public void Bet(int bet, Pot mainPot)
        {
            if (this.ChipStack <= bet)
            {
                this.AllIn(mainPot);
                return;
            }
            this.ChipStack -= bet;
            this.amountInPot += bet;
            mainPot.Add(bet);
            mainPot.setMaximumAmount(this.amountInPot);
            mainPot.MinimumRaise = bet;
            this.Message = "Bet " + bet.ToString("c0");
            this.SimplifiedMessage = "BET " + bet;
        }
        public void Bet(int bet, Pot mainPot, int index)
        {
            if (this.ChipStack <= bet)
            {
                this.AllIn(mainPot,index);
                return;
            }
            this.ChipStack -= bet;
            this.amountInPot += bet;
            mainPot.Add(bet);
            mainPot.setMaximumAmount(this.amountInPot);
            mainPot.MinimumRaise = bet;
            this.Message = "Bet " + bet.ToString("c0");
            this.SimplifiedMessage = "BET " + bet;
            mainPot.AgressorIndex = index;
        }
        //all in, bet remaining chipstack
        public void AllIn(Pot mainPot)
        {
            this.AmountContributed = this.ChipStack;
            if (mainPot.MinimumAllInAmount == 0)
            {
                mainPot.AmountInPotBeforeAllIn = mainPot.Amount;
                mainPot.MinimumAllInAmount = this.ChipStack;
            }
            else if (this.chipStack < mainPot.MinimumAllInAmount)
            {
                mainPot.MinimumAllInAmount = this.ChipStack;
            }
            if (this.ChipStack > mainPot.MinimumRaise)
                mainPot.MinimumRaise = this.ChipStack;
            mainPot.AddPlayer(this);
            mainPot.Add(this.ChipStack);
            this.amountInPot += this.ChipStack;
            this.ChipStack = 0;
            if (this.amountInPot > mainPot.getMaximumAmountPutIn())
                mainPot.setMaximumAmount(this.amountInPot);
            this.Message = "I'm All-In";
            this.SimplifiedMessage = "ALL IN";
        }
        public void AllIn(Pot mainPot,int index)
        {
            this.AmountContributed = this.ChipStack;
            if (mainPot.MinimumAllInAmount == 0)
            {
                mainPot.AmountInPotBeforeAllIn = mainPot.Amount;
                mainPot.MinimumAllInAmount = this.ChipStack;
            }
            else if (this.chipStack < mainPot.MinimumAllInAmount)
            {
                mainPot.MinimumAllInAmount = this.ChipStack;
            }
            if (this.ChipStack > mainPot.MinimumRaise)
                mainPot.MinimumRaise = this.ChipStack;
            mainPot.AddPlayer(this);
            mainPot.Add(this.ChipStack);
            this.amountInPot += this.ChipStack;
            this.ChipStack = 0;
            if(this.amountInPot>mainPot.getMaximumAmountPutIn())
                mainPot.setMaximumAmount(this.amountInPot);
            this.Message = "I'm All-In";
            this.SimplifiedMessage = "ALL IN";
            mainPot.AgressorIndex = index;
        }
        //reset the individual players
        public void Reset()
        {
            this.amountInPot = 0;
            this.folded = false;
            this.InitialStack = this.ChipStack;
            this.myHand.Clear();
            this.Message = "";
            this.SimplifiedMessage = "";
        }
        //collect the winnings if the player wins
        public void CollectMoney(Pot mainPot)
        {
            this.ChipStack += mainPot.Amount;
            this.Message = this.Name + " wins the pot!";
        }
        //set isBusted to true if the player busted out
        public void Leave()
        {
            if (this.ChipStack != 0)
                throw new InvalidOperationException();
            this.Message = this.Name + " busted out!";
            this.isbusted = true;
        }
        
    }
}
