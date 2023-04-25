using HorrorFox;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            if (other.CompareTag("useKeyZone"))         //pit‰‰ laitata niin, ett‰ tulee prompt, ja vasta kun on painettu sit‰ nappia, tehd‰‰n seuravaa asia...
            {
                Debug.Log("used key");
                Door door;
                try
                {
                    door = other.transform.root.GetComponent<Door>();
                }
                catch
                {
                    Debug.Log("parenpt object has no door scriot");
                    return;
                }

                currentKeys[0].UseKey(door);
                currentKeys.Remove(currentKeys[0]);
            }
        }

    }
}

