namespace TexasHoldem.AI.SmokinAcesPlayer
{
    using System.Collections.Generic;
    using System.Linq;
    using Logic.Cards;

    public static class Extensions
    {
        public static Card Deal(this List<Card> deck)
        {
            var card = deck.ElementAt(deck.Count - 1);
            deck.RemoveAt(deck.Count - 1);
            return card;
        }

        public static void CustomRemoveRange(this List<Card> deck, List<Card> cardsToRemove)
        {
            foreach (var card in cardsToRemove)
            {
                deck.Remove(card);
            }
        }
    }
}