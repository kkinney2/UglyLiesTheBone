using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class Locomotion : MonoBehaviour
    {
        Hand hand_L;
        Hand hand_R;

        Rigidbody rb;

        [Range(1, 100)]
        public int speed = 10;

        //public ISteamVR_Action_Vector2 = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("Movement");

        private void Awake()
        {
            hand_L = GameObject.Find("LeftHand").GetComponent<Hand>();
            hand_R = GameObject.Find("RightHand").GetComponent<Hand>();

            rb = GetComponent<Rigidbody>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float horizontalAxis = Input.GetAxis("Joystick_X");
            float verticalAxis = Input.GetAxis("Joystick_Y");

            rb.AddForce(new Vector3(horizontalAxis, verticalAxis, 0));
        }
    }
}
