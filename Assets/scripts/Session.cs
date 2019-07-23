using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WarGame {
    /// <summary>
    /// The Game class is a Unity-based manager.
    /// It maintains the state of the game and coordinates the various parts
    /// </summary>
    public class Session : MonoBehaviour {
        // Prefab for game. Create instant to generate container of cards 
        public Deck deckPrefab;
        // The players' personal pile of cards
        public List<Stock> stocks;
        // The current cards on the field and in play for the round
        public List<List<CardData>> pool;
        protected GameState curGameState;
        protected RoundState prevRoundState, curRoundState;
        bool trigger, advanceState;
        int curStep, amountToDraw;
        public Stock roundWinner;
        public event EventHandler OnRoundStep;

        /*
         * Game Cycle
         *  * Application launches
         *      * Intro Screen
         *      * Main Menu (Start Game)
         *      * Game
         *          * Initialize Session
         *          * Rounds (Players turns are taken simultaneously)
         *              * Both players draw
         *              * Both players reveal
         *              * Round winner determined OR WAR
         *                  * This may trigger the end of the session as well
         */
        // Start is called before the first frame update
        void Start() {
            // trigger = false;
            // forwardStep = false;
            pool = new List<List<CardData>>();
            amountToDraw = 1;
            curGameState = GameState.Initialize;
            curRoundState = RoundState.Start;
            // Subscribe to OnRoundStep event
            OnRoundStep += a_OnRoundStep;
        }

        /* 
         * TODO- I would like to decouple SplitDeck() from the Update function
         * ------------------------------------------
         * What's needed for both the rounds and the game is a state machine. 
         * I need the capacity to walk through a cycle of states and control how
         * the linger.
         * For round this is start -> draw -> reveal -> determine -> start
         * For game this is initialize -> play -> end
         */

        // Update is called once per frame
        void Update() {
            AdvanceRoundState();
            UpdateGame();

        }
        void AdvanceRoundState() {
            if (advanceState) {
                advanceState = false;
                var lastLiveState = curRoundState;
                if (lastLiveState == RoundState.Wait) {
                    lastLiveState = prevRoundState;
                }
                switch (lastLiveState) {
                    case RoundState.Start:
                        curRoundState = RoundState.Draw;
                        break;
                    case RoundState.Draw:
                        curRoundState = RoundState.Reveal;
                        break;
                    case RoundState.Reveal:
                        curRoundState = RoundState.Evaluate;
                        break;
                    case RoundState.Evaluate:
                        // curRoundState = RoundState.End;
                        curRoundState = RoundState.Draw;
                        break;
                    case RoundState.End:
                        curRoundState = RoundState.Start;
                        break;
                    case RoundState.Wait:
                        // This should never happen. "Wait" is an interim state which occurs between triggers
                        break;
                    case RoundState.Pause:
                        break;
                    default:
                        break;
                }
            }
        }
        void UpdateGame() {
            switch (curGameState) {
                case GameState.Initialize:
                    InitializeGame();
                    break;
                case GameState.Play:
                    UpdateRound();
                    break;
                case GameState.End:
                    break;
                case GameState.Pause:
                    break;
                default:
                    break;
            }
        }
        void InitializeGame() {
            SplitDeck();
            curGameState = GameState.Play;
        }

        void SplitDeck() {
            // Create a full deck of cards. Game Object doesn't matter, just the attached script.
            var deck = Instantiate(deckPrefab, Vector3.zero, Quaternion.identity).GetComponent<Deck>();
            var script = deck.GetComponent<Deck>();
            var length = script.Count();
            // Pass a card to each player until the full deck has been ran through
            for (var i = 0; i < length; i++) {
                if (i % 2 == 0) {
                    stocks[0].Add(script.Deal());
                } else {
                    stocks[1].Add(script.Deal());
                }
            }
            Destroy(deck);
        }

        public void NextStep() {
            OnRoundStep(this, EventArgs.Empty);
        }
        public void a_OnRoundStep(object sender, EventArgs e) {
            Debug.Log("Event Triggered");
            advanceState = true;
        }
        public void Draw(List<Stock> stocks, int amountToDraw = 1) {
            foreach (Stock stock in stocks) {
                stock.Draw(amountToDraw);
            }
        }
        public void Reveal(List<Stock> stocks) {
            foreach (Stock stock in stocks) {
                stock.Reveal();
                // var list = new List<CardData>();
                // list.AddRange(stock.CurCard);
                // pool.Add(list);
                // stock.CurCard.Clear();
            }
        }
        public void Destroy(List<Stock> stocks) {
            foreach (Stock stock in stocks) {
                stock.Destroy();
            }
        }
        public bool Evaluate(List<Stock> stocks) {
            /*
             * Assessment Step
             * - Gather the active card for each player
             * - Convert active card to value
             * - Compare the values for each player
             * - Set and Announce the winner
             */
            Stock selectedStock = null;
            var isTie = false;
            foreach (Stock stock in stocks) {
                if (selectedStock != null) {
                    // Compare stocks by top card
                    var result = selectedStock.GetTopCard().CompareRank(stock.GetTopCard());
                    result = 0;
                    if (result == 0) {
                        // Tie!
                        isTie = true;
                        selectedStock = null;
                    } else if (result < 0) {
                        // selectedStock is less than stock
                        // Replace selectedStock with new highestValue
                        selectedStock = stock;
                    }
                    // if result > 0, selectedStock remains highest. No change.
                } else {
                    // Set initial value
                    selectedStock = stock;
                }
            }
            if (!isTie)
            {
                Debug.Log(selectedStock.name + " has won the round with a " + selectedStock.GetTopCard().ToString());
            }
            // Transfer in-play cards to neutral pool
            foreach (Stock stock in stocks) {
                // Transfer cards to winner
                var list = new List<CardData>();
                list.AddRange(stock.CurCard);
                pool.Add(list);
                stock.CurCard.Clear();
            }
            // Transfer evaluated cards to a pool
            if (!isTie) {
                foreach (List<CardData> list in pool) {
                    selectedStock.Add(list);
                }
                pool.Clear();

                // Destroy game objects
                var cards = GameObject.FindGameObjectsWithTag("Card");
                foreach (var card in cards) {
                    var script = card.GetComponent<CardDisplay>();
                    script.destination = selectedStock.GetDestination();
                    script.FlipCard();
                }
            }
            return isTie;
        }
        public void UpdateRound() {
            switch (curRoundState) {
                case RoundState.Start:
                    Debug.Log("Start State Executing");
                    WaitForInput();
                    break;
                case RoundState.Draw:
                    Debug.Log("Draw State Executing");
                    Draw(stocks, amountToDraw);
                    WaitForInput();
                    break;
                case RoundState.Reveal:
                    Debug.Log("Reveal State Executing");
                    Reveal(stocks);
                    WaitForInput();
                    break;
                case RoundState.Evaluate:
                    Debug.Log("Evaluate State Executing");
                    // var values = Evaluate(stocks);
                    var isTie = Evaluate(stocks);
                    if (isTie) {
                        amountToDraw = 3;
                    } else {
                        amountToDraw = 1;
                        Destroy(stocks);
                    }
                    WaitForInput();
                    break;
                case RoundState.End:
                    Debug.Log("End State Executing");
                    WaitForInput();
                    break;
                case RoundState.Wait:
                    break;
                case RoundState.Pause:
                    break;
                default:
                    break;
            }
            curStep++;
            if (curStep > 2) {
                curStep = 0;
            }

        }
        public void WaitForInput() {
            prevRoundState = curRoundState;
            curRoundState = RoundState.Wait;
        }
    }
}