using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WarGame
{
    public class Rules
    {
        public static Dictionary<Rank, int> rankWeight = new Dictionary<Rank, int>() {
            {Rank.Ace, 14},
            {Rank.Two, 2},
            {Rank.Three, 3},
            {Rank.Four, 4},
            {Rank.Five, 5},
            {Rank.Six, 6},
            {Rank.Seven, 7},
            {Rank.Eight, 8},
            {Rank.Nine, 9},
            {Rank.Ten, 10},
            {Rank.Jack, 11},
            {Rank.Queen, 12},
            {Rank.King, 13}
        };
    }
}
