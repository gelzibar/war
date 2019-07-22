using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace WarGame
{
    public class Stock : MonoBehaviour
    {
        [SerializeField]
        public List<string> cardNames;
        // Web calls player's facedown deck a "packet"
        public List<CardData> cards;
        private List<CardData> curCard;
        public Vector3 position;
        public GameObject displayedCard;
        private int positionOffset;
        public List<CardData> CurCard
        {
            get { return curCard; }
            set { curCard = value; }
        }
        public GameObject prefabCard;
        // Start is called before the first frame update
        void Start()
        {
            cards = new List<CardData>();
            curCard = new List<CardData>();
            cardNames = new List<string>();
        }

        // Update is called once per frame
        void Update()
        {
        }
        public void Draw(int amount)
        {
            var drawn = new List<CardData>();
            drawn.AddRange(cards.GetRange(0, amount));
            curCard = drawn;
            cards.RemoveRange(0, amount);

            var pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            var x = (Mathf.Abs(transform.position.x) + positionOffset) * Mathf.Sign(transform.position.x);
            foreach (CardData card in curCard)
            {
                var isHidden = true;
                if (card == curCard.Last())
                {
                    isHidden = false;
                }
                CreateImage(card, new Vector3(x, pos.y, pos.z), isHidden);
                x = (Mathf.Abs(x) + Mathf.Abs(transform.position.x)) * Mathf.Sign(transform.position.x);
                positionOffset++;
            }
        }
        public void Reveal()
        {
            var list = GameObject.FindGameObjectsWithTag("Card");
            foreach (var card in list)
            {
                var display = card.GetComponent<CardDisplay>();
                if (!display.isHidden)
                {
                    display.FlipCard();
                }

            }

        }
        public void AnimateReveal()
        {
            foreach (CardData card in curCard)
            {
                // card.To
            }
        }
        public void Destroy()
        {
            positionOffset = 0;
            Destroy(displayedCard, 4f);
        }
        public void Add(List<CardData> cardsToAdd)
        {
            cards.AddRange(cardsToAdd);
        }
        public void CreateImage(CardData card, Vector3 pos, bool isHidden)
        {
            var rot = Quaternion.identity;
            // if (isHidden)
            // {
            //     rot = Quaternion.Euler(0, 180f, 0);
            // }
            rot = Quaternion.Euler(270f, 180f, 0);
            var obj = Instantiate(prefabCard, pos, rot);
            obj.name = this.name + " Card";
            displayedCard = obj;
            var rank = obj.transform.Find("Rank");
            var suit = obj.transform.Find("Suit");
            var rankResource = Resources.Load<Sprite>("rank-suit/" + card.rank.ToString());
            var suitResource = Resources.Load<Sprite>("rank-suit/" + card.suit.ToString());
            rank.GetComponent<SpriteRenderer>().sprite = rankResource;
            suit.GetComponent<SpriteRenderer>().sprite = suitResource;
            var script = obj.GetComponent<CardDisplay>();
            script.owner = this.gameObject;
            obj.transform.parent = this.transform;
            // script.TriggerDealAnimation();
        }
        public Vector3 GetDestination()
        {
            var destination = transform.GetChild(0);
            return destination.position;
        }
    }
}