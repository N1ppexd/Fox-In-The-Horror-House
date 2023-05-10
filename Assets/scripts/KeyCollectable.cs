using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace HorrorFox.Fox.Keys
{
    public class KeyCollectable : MonoBehaviour
    {

        [SerializeField] private ParticleSystem takeKeyParticle, useKeyParticle;

        [SerializeField] private Transform target; //target on se, jota seurataan, eli pelaaja....



        private bool hasBeenTaken;

        //mill‰ offsetilla avain seuraa pelaajaa..
        [SerializeField] private Vector3 keyMovementOffset;
        [SerializeField] private float movementSpeed, smoothSpeed;//smoothspeed on se, kuinka nopeaa avain seuraa pelaajaa....

        [SerializeField] private GameObject keyTrigger;


        [SerializeField] private FoxInteract foxInteract;

        [SerializeField] private MeshRenderer keyMesh;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("fox"))
            {
                hasBeenTaken = true;
                takeKeyParticle.Play();
                keyTrigger.SetActive(false);

                foxInteract.currentKeys.Add(this);
                foxInteract.StopPressingInteractButton(); //kettu lopettaa kaiken interaktion huonekalujen kanssa...

            }
        }


        private void LateUpdate()
        {
            if(!hasBeenTaken)
                return;

            transform.position = Vector3.Lerp(transform.position, target.position + keyMovementOffset, movementSpeed * Time.deltaTime);
        }


        public void UseKey(Door door)//kun avainta k‰ytet‰‰n...
        {
            keyMesh.enabled = false;
            door.OpenDoor();
            useKeyParticle.Play();
            Destroy(gameObject, 1f);
        }
    }
}

