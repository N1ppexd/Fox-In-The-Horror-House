using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HorrorFox.Fox.Keys
{
    public class KeyCollectable : MonoBehaviour
    {

        [SerializeField] private ParticleSystem takeKeyParticle;

        [SerializeField] private Transform target; //target on se, jota seurataan, eli pelaaja....



        private bool hasBeenTaken;

        //mill‰ offsetilla avain seuraa pelaajaa..
        [SerializeField] private Vector3 keyMovementOffset;
        [SerializeField] private float movementSpeed, smoothSpeed;//smoothspeed on se, kuinka nopeaa avain seuraa pelaajaa....

        [SerializeField] private GameObject keyTrigger;


        [SerializeField] private FoxInteract foxInteract;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("fox"))
            {
                hasBeenTaken = true;
                takeKeyParticle.Play();
                keyTrigger.SetActive(false);

                foxInteract.currentKeys.Add(this);
            }
        }


        private void LateUpdate()
        {
            if(!hasBeenTaken)
                return;

            transform.position = Vector3.Lerp(transform.position, target.position + keyMovementOffset, movementSpeed * Time.deltaTime);
        }


        public void UseKey()//kun avainta k‰ytet‰‰n...
        {

        }
    }
}

