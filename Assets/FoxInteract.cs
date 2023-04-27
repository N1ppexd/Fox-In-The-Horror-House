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

        private bool isInInteractRadius;//kun ollaan alueella, jossa voidaan painaa interact nappia...


        private void Awake()
        {
            inputMaster = new InputMaster();
        }

        private void OnEnable()
        {
            inputMaster.Enable();

            //pit‰‰ laitata se juttu t‰h‰n ewtt‰ prompti toimiI!!!!

            inputMaster.Player.Interact.performed += _ => PressPromptButton();

        }
        private void OnDisable()
        {
            inputMaster.Disable();

            inputMaster.Player.Interact.performed -= _ => PressPromptButton();
        }


        private void Start()
        {
            currentKeys = new List<KeyCollectable>();
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
                    promptCanvas = door.transform.Find("PropmtPanel").gameObject;
                }
                catch
                {
                    Debug.Log("parenpt object has no door scriot");
                    return;
                }

                isInInteractRadius = true;

                DisplayPrompt();
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("useKeyZone"))         //pit‰‰ laitata niin, ett‰ tulee prompt, ja vasta kun on painettu sit‰ nappia, tehd‰‰n seuravaa asia...
            {
                if (isInInteractRadius)
                {
                    isInInteractRadius = false;

                    promptCanvas.SetActive(false);
                }

            }
        }


        void DisplayPrompt()
        {
            isShowingPrompt = true;
            promptCanvas.SetActive(true);
        }

        
        /// <summary>
        /// kun painetaan prompt buttonia...
        /// </summary>
        void PressPromptButton()
        {
            if (!isInInteractRadius)
                return;

            Debug.Log("painetaan juttua");

            promptCanvas.SetActive(false);
            currentKeys[0].UseKey(door);
            currentKeys.Remove(currentKeys[0]);
        }


    }
}

