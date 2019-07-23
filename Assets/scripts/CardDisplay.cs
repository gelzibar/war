using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WarGame
{
    public class CardDisplay : MonoBehaviour
    {
        public Vector3 destination;
        public float destinationOffset;
        public CardData data;
        private Transform myTransform;
        private Animation animationControl;
        private float rotationIncrement;
        private bool isFlipping;
        private int flipFrames, maxFlipFrames;
        private float flipFinalAngle, defaultFinalAngle;
        public Vector3 flipTarget;
        public bool isInPlay, isHidden;
        public GameObject owner;
        public float rotateY, vertSpeed;
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Card Start Executed");
            myTransform = this.GetComponent<Transform>();
            maxFlipFrames = 60;
            flipFrames = maxFlipFrames;
            defaultFinalAngle = 180f;

            // Set amount to rotate per frame
            var time = 1f;
            var frames = 60;
            rotationIncrement = 180 / (time * frames);
            var search = "Stock 1";
            if (this.name == "Stock 2 Card")
            {
                search = "Stock 2";
            }
            owner = GameObject.Find(search);

            animationControl = GameObject.Find("AnimationManager").GetComponent<Animation>();
            destination = Animation.AssignTargets(this);
            var x = (Mathf.Abs(destination.x) + destinationOffset) * Mathf.Sign(destination.x);
            destination = new Vector3(x, destination.y, destination.z);

            rotateY = 5;
            vertSpeed = 0.5f;

        }

        // Update is called once per frame
        void Update()
        {
            // var angle = new Vector3(0, transform.rotation.eulerAngles.y + rotationIncrement, 0);
            if (isFlipping)
            {
                AnimateCardFlip();
            }
            // var target = new Vector3(2, 0, transform.position.z);
            AnimateCardToss(destination);

        }
        public void FlipCard()
        {
            isFlipping = true;
            flipFinalAngle = defaultFinalAngle;
        }
        public void AnimateCardToss(Vector3 target)
        {
            var pos = Vector3.MoveTowards(myTransform.position, target, 0.05f);
            // transform.Translate(pos, Space.World);
            myTransform.position = pos;
        }
        public void Bounce()
        {
            Debug.Log("Bounce Started");
            // if(flipFinalAngle == defaultFinalAngle)
            // {
            //     flipTarget = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            // }
            // var target = new Vector3(flipTarget.x, flipTarget.y, flipTarget.z);
            // if (flipFinalAngle < (defaultFinalAngle / 2))
            // {
            //     target = new Vector3(flipTarget.x, flipTarget.y - 1, flipTarget.z);
            // }

            // if (flipFinalAngle > 0)
            // {
            //     // var angle = new Vector3(0, rotateY, 0);
            //     var speed = vertSpeed * Time.deltaTime;
            //     // var pos = Vector3.MoveTowards(transform.position, target, speed);
            //     var up = Vector3.up * speed;
            //     transform.Translate(up, Space.World);
            //     // transform.position = pos;
            //     // transform.Rotate(0, rotateY, 0, Space.Self);
            //     flipFinalAngle -= rotateY;
            // }
            // else
            // {
            //     isFlipping = false;
            //     flipFrames = maxFlipFrames;
            //     flipFinalAngle = defaultFinalAngle;
            // }
            Debug.Log("Bounce Ended");
        }
        public void AnimateCardFlip()
        {
            // 0.35715 - half width of card
            if (flipFinalAngle > 0)
            {
                var angle = new Vector3(0, rotateY, 0);
                myTransform.Rotate(0, rotateY, 0, Space.Self);
                flipFinalAngle -= rotateY;
            }
            else
            {
                isFlipping = false;
                flipFrames = maxFlipFrames;
                flipFinalAngle = defaultFinalAngle;
            }
        }
        public void AnimateCardFlip2()
        {
            // 0.35715
            // .178575
            var VerticalTravel = 0.35715f;
            var vertSpeed = -1 * (VerticalTravel / (defaultFinalAngle / 2));
            if (flipFinalAngle > (defaultFinalAngle / 2))
            {
                vertSpeed *= -10;
            }
            if (flipFinalAngle > 0)
            {
                var angle = new Vector3(0, rotationIncrement, 0);
                var pos = new Vector3(myTransform.position.x, myTransform.position.y + vertSpeed, myTransform.position.z + vertSpeed);
                myTransform.Translate(Vector3.up * vertSpeed);
                myTransform.Rotate(angle);
                flipFinalAngle -= rotationIncrement;
                flipFrames--;
            }
            else
            {
                isFlipping = false;
                flipFrames = maxFlipFrames;
                flipFinalAngle = defaultFinalAngle;
            }
        }
    }
}