using HorrorFox;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HorrorFox.Fox.Keys
{
    public class FoxInteract : MonoBehaviour
    {

        public List<KeyCollectable> currentKeys;

        [SerializeField] private PlayerInput playerInput;

        [SerializeField] private GameObject promptCanvas;

        private bool isShowingPrompt;//n‰ytet‰‰n promptia kun t‰m‰ on true....

        private InputMaster inputMaster;



        private void OnEnable()
        {
            inputMaster.Enable();

            //pit‰‰ laitata se juttu t‰h‰n ewtt‰ prompti toimiI!!!!

        }
        private void OnDisable()
        {
            inputMaster.Disable();
        }


        private void Start()
        {
            currentKeys = new List<KeyCollectable>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        Door door;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("useKeyZone"))         //pit‰‰ laitata niin, ett‰ tulee prompt, ja vasta kun on painettu sit‰ nappia, tehd‰‰n seuravaa asia...
            {
                Debug.Log("used key");
                
                try
                {
                    door = other.transform.root.GetComponent<Door>();
                }
                catch
                {
                    Debug.Log("parenpt object has no door scriot");
                    return;
                }

                DisplayPrompt();
                
            }
        }


        void DisplayPrompt()
        {
            isShowingPrompt = true;
            promptCanvas.SetActive(true);
        }

        void PressPromptButton()
        {
            currentKeys[0].UseKey(door);
            currentKeys.Remove(currentKeys[0]);
        }


    }
}

