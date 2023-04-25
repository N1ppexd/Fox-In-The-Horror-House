using HorrorFox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox.Fox.Keys
{
    public class FoxInteract : MonoBehaviour
    {

        public List<KeyCollectable> currentKeys;

        private void Start()
        {
            currentKeys = new List<KeyCollectable>();
        }

        // Update is called once per frame
        void Update()
        {

        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("useKeyZone"))
            {
                currentKeys[0].UseKey();
                currentKeys.Remove(currentKeys[0]);
            }
        }

    }
}

