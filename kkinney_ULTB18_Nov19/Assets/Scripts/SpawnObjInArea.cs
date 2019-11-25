using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to access the VR scripts
// ie. Hand
namespace Valve.VR.InteractionSystem
{
    public class SpawnObjInArea : MonoBehaviour
    {
        [Header("Object to Spawn")]
        public GameObject objToSpawn;

        [Tooltip("If unassigned, will use attached gameobject")]
        public GameObject spawnArea;

        bool canSpawn = false;

        Hand hand1;
        Hand hand2;

        GameObject obj1;
        GameObject obj2;

        // Start is called before the first frame update
        void Start()
        {
            // If no area has been assigned, use the object this is attached to
            if (spawnArea == null)
            {
                spawnArea = this.gameObject;
            }

            GetComponent<Collider>().isTrigger = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (canSpawn)
            {
                // Place obj in range to be picked up
                if (hand1 != null)
                {
                    obj1.transform.position = hand1.gameObject.transform.position;
                }
                if (hand2 != null)
                {
                    obj2.transform.position = hand2.gameObject.transform.position;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Hand>())
            {
                canSpawn = true;

                // Give a temp reference for the hand
                if (hand1 != null)
                {
                    hand1 = other.gameObject.GetComponent<Hand>();
                    obj1 = Instantiate(objToSpawn);
                }
                else
                {
                    hand2 = other.gameObject.GetComponent<Hand>();
                    obj2 = Instantiate(objToSpawn);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<Hand>())
            {
                canSpawn = false;

                // Remove temp reference for the hand
                if (other.gameObject.GetComponent<Hand>() == hand1)
                {
                    hand1 = null;
                    Destroy(obj1);
                }
                else
                {
                    hand2 = null;
                    Destroy(obj2);
                }
            }
        }

    }
}
