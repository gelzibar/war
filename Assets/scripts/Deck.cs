using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WarGame
{
    public class Deck : MonoBehaviour
    {
        protected List<CardData> cards;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            Initialize();
            Shuffle();
            WriteToFile(cards);
        }
        void Update()
        {

        }

        public int Count()
        {
            return this.cards.Count;
        }

        void Initialize()
        {
            cards = new List<CardData>();
            GenerateSuit(Suit.Club);
            GenerateSuit(Suit.Diamond);
            GenerateSuit(Suit.Heart);
            GenerateSuit(Suit.Spade);
        }

        static void WriteToFile(List<CardData> cardList)
        {
            string[] lines = new string[52];
            var index = 0;
            foreach (CardData card in cardList)
            {
                lines[index] = card.ToString();
                index++;
            }
            System.IO.File.WriteAllLines(@"./CardSequence.txt", lines);
        }

        void Shuffle()
        {
            var initialDeck = cards;
            var shuffledDeck = new List<CardData>();
            var rand = new System.Random();
            var loopLength = initialDeck.Count;
            for (var i = 0; i < loopLength; i++)
            {
                var length = initialDeck.Count;
                var randIndex = rand.Next(0, length);
                shuffledDeck.Add(initialDeck[randIndex]);
                initialDeck.Remove(initialDeck[randIndex]);
            }
            cards = shuffledDeck;

        }

        public List<CardData> Deal(int amountToDeal = 1)
        {
            var dealt = new List<CardData>();
            for (var i = 0; i < amountToDeal; i++)
            {
                dealt.Add(cards[0]);
                cards.RemoveAt(0);
            }
            return dealt;
        }

        void GenerateSuit(Suit suit)
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                CardData newCard = new CardData(rank, suit);
                // newCard.Initialize(rank, suit);
                cards.Add(newCard);
            }
        }
    }
}