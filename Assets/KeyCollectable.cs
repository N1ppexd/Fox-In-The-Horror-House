using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HorrorFox
{
    public class KeyCollectable : MonoBehaviour
    {

        [SerializeField] private ParticleSystem takeKeyParticle;

        [SerializeField] private Transform target; //target on se, jota seurataan, eli pelaaja....



        private bool hasBeenTaken;

        //millä offsetilla avain seuraa pelaajaa..
        [SerializeField] private Vector3 keyMovementOffset;
        [SerializeField] private float movementSpeed, smoothSpeed;//smoothspeed on se, kuinka nopeaa avain seuraa pelaajaa....


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("fox"))
            {
                hasBeenTaken = true;
                takeKeyParticle.Play();
            }
        }


        private void LateUpdate()
        {
            if(!hasBeenTaken)
                return;

            transform.position = Vector3.Lerp(transform.position, target.position + keyMovementOffset, movementSpeed * Time.deltaTime);
        }
    }
}

