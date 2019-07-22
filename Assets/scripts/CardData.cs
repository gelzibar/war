using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WarGame
{
    public class CardData
    {
        public Suit suit;
        public Rank rank;

        public CardData (Rank rank, Suit suit) {
            this.rank = rank;
            this.suit = suit;
        }
        public void Initialize(Rank rank, Suit suit)
        {
            this.rank = rank;
            this.suit = suit;
        }
        public override string ToString()
        {
            return rank.ToString() + " of " + suit.ToString() + "s";
        }
        public Rank GetRank()
        {
            return rank;
        }
    }
}
