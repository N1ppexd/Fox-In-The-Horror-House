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

        private bool isShowingPrompt;//n�ytet��n promptia kun t�m� on true....

        private InputMaster inputMaster;

        private bool isInInteractRadius;//kun ollaan alueella, jossa voidaan painaa interact nappia...

        [SerializeField] private Transform draggerObj; //ketun kuono..


        private bool 
            isDoor,         //t�m� in true, kun pit�� k�ytt�� avainta
            isInteractableThing, //t�m� on true, kun yritet��n painaa jotain nappia tai jotain..
            isDragObj;      //t�m� on true


        [SerializeField] private Rigidbody rb;

        private float originalMass;


        private void Awake()
        {
            inputMaster = new InputMaster();
        }

        private void OnEnable()
        {
            inputMaster.Enable();

            //pit�� laitata se juttu t�h�n ewtt� prompti toimiI!!!!

            inputMaster.Player.Interact.performed += _ => PressPromptButton();
            inputMaster.Player.Interact.canceled += _ => StopPressingInteractButton();

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
        InteractObj interactObj; // t�m� on scripti, joka handlaa kaikki perus interaktiot paitsi oven ja esineiden raahamisen..

        /// <summary>
        /// transform that is dragged by the fox..
        /// </summary>
        Transform dragObj;


        GameObject explanationCanvas;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("useKeyZone"))         //pit�� laitata niin, ett� tulee prompt, ja vasta kun on painettu sit� nappia, tehd��n seuravaa asia...
            {
                Debug.Log("used key");

                
                
                try
                {
                    door = other.transform.root.GetComponent<Door>();
                    promptCanvas = door.transform.Find("PropmtPanel").gameObject;
                    explanationCanvas = door.transform.Find("explanationPanel").gameObject;
                }
                catch
                {
                    Debug.Log("parenpt object has no door scriot");
                    return;
                }

                isInInteractRadius = true;

                isDoor = true;

                if(currentKeys.Count > 0)
                    DisplayPrompt();
                else
                {
                    explanationCanvas.SetActive(true);
                }
                
            }

            if (currentKeys.Count > 0)      //EI VOI INTERACTATA, JOS ON AVAIMIA
                return;

            isDoor = false;

            if (other.CompareTag("interactZone"))
            {
                try
                {
                    promptCanvas = other.transform.Find("PropmtPanel").gameObject;

                    interactObj = other.gameObject.GetComponent<InteractObj>(); //xdd
                }
                catch
                {
                    Debug.Log("parenpt object has no door scriot");
                    return;
                }

                isInInteractRadius = true;

                isDragObj = false;
                isInteractableThing = true;

                DisplayPrompt();
            }

            if (other.CompareTag("interactObjZone"))
            {
                try
                {
                    promptCanvas = other.transform.parent.transform.Find("PropmtPanel").gameObject;
                }
                catch
                {
                    Debug.Log("parenpt object has no door scriot");
                    return;
                }

                isInInteractRadius = true;
                isDragObj = true;
                isInteractableThing = false;

                dragObj = other.transform.parent;

                DisplayPrompt();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("useKeyZone"))         //pit�� laitata niin, ett� tulee prompt, ja vasta kun on painettu sit� nappia, tehd��n seuravaa asia...
            {
                Debug.Log("interactZone");

                if (isInInteractRadius)
                {
                    isInInteractRadius = false;

                    promptCanvas.SetActive(false);
                    explanationCanvas.SetActive(false);
                }
                isDoor = false;
            }

            if (other.CompareTag("interactZone"))   //interactzone on zone, jossa interaktataan esim nappien kaa. interactobj zone on esineet, joita voi ty�nt��..
            {
                Debug.Log("interactZone");

                if (isInInteractRadius)
                {
                    isInInteractRadius = false;

                    promptCanvas.SetActive(false);
                }
            }

            if (other.CompareTag("interactObjZone"))   //interactzone on zone, jossa interaktataan esim nappien kaa. interactobj zone on esineet, joita voi ty�nt��..
            {
                if (isInInteractRadius)
                {
                    isInInteractRadius = false;

                    promptCanvas.SetActive(false);

                    StopPressingInteractButton();
                }
            }
        }


        void DisplayPrompt()
        {
            isShowingPrompt = true;
            promptCanvas.SetActive(true);
        }


        private ConfigurableJoint joint;

        /// <summary>
        /// kun painetaan prompt buttonia...
        /// </summary>
        void PressPromptButton()
        {
            if (!isInInteractRadius)
                return;

            Debug.Log("painetaan juttua");

            promptCanvas.SetActive(false);
            if (isDoor)
            {
                currentKeys[0].UseKey(door);

                currentKeys.Remove(currentKeys[0]);
                return;
            }

            if(currentKeys.Count > 0)      //EI VOI INTERACTATA, JOS ON AVAIMIA
                    return;

            if (isInteractableThing)
            {
                interactObj.Interact(); //tehd��n n�in...
                return;
            }

            DragObj dragObjScript = dragObj.GetComponent<DragObj>();


            dragObjScript.draggerObj = draggerObj;
            //dragObjScript.SetDragOffset();

            dragObjScript.isBeingDragged = true;


            joint = dragObj.GetComponent<ConfigurableJoint>();
            joint.connectedBody = rb;

            joint.configuredInWorldSpace = true;

            originalMass = dragObj.GetComponent<Rigidbody>().mass;

            dragObj.GetComponent<Rigidbody>().mass = 0.1f;

            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Free;
            joint.zMotion = ConfigurableJointMotion.Locked;

            joint.angularYMotion = ConfigurableJointMotion.Limited;

            joint.angularXMotion = ConfigurableJointMotion.Limited;

            joint.angularZMotion = ConfigurableJointMotion.Limited;


            //jotain mill� saadaan esine liikkumaan ketun mukana ja lopettamaan liikkuminen, kun lopetetaan painaminen...
        }

        /// <summary>
        /// t�m� tulee, jos liikutetaan jotain esinett�, ja lopetetaan painaminen. T�m� tulee my�s, kun otetaan avain, koska avaimen kanssa ei voi interactata huonekalujen kanssa...
        /// </summary>
        public void StopPressingInteractButton()
        {
            
            try
            {
                DragObj dragObjScript = dragObj.GetComponent<DragObj>();


                dragObjScript.isBeingDragged = false;

                dragObj.GetComponent<Rigidbody>().mass = originalMass;

                joint.connectedBody = null;

                joint.xMotion = ConfigurableJointMotion.Free;
                joint.yMotion = ConfigurableJointMotion.Free;
                joint.zMotion = ConfigurableJointMotion.Free;

                joint.angularYMotion = ConfigurableJointMotion.Free;

                joint.angularXMotion = ConfigurableJointMotion.Free;

                joint.angularZMotion = ConfigurableJointMotion.Free;

            }
            catch
            {
                return;
            }

            


            //Destroy(joint);
        }


    }
}

