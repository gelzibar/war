using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace WarGame
{
    public class Animation : MonoBehaviour
    {
        public GameObject playPosition1, playPosition2;
        public static GameObject player1, player2;

        public static Vector3 playVector1, playVector2;
        // Start is called before the first frame update
        void Start()
        {
            playVector1 = playPosition1.GetComponent<Transform>().position;
            playVector2 = playPosition2.GetComponent<Transform>().position;

        }

        // Update is called once per frame
        void Update()
        {

        }
        public static Vector3 AssignTargets(CardDisplay script)
        {
            var target = Vector3.zero;
            var player2 = GameObject.Find("Stock 2").name;
            if (!script.isInPlay)
            {
                target = playVector1;
                if (script.owner.name == player2)
                {
                    target = playVector2;
                }
            }
            return target;
        }
    }
}