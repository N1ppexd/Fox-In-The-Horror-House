using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox
{
    public class BoxShatter : MonoBehaviour
    {

        [SerializeField] Rigidbody[] rigidbodies;

        [SerializeField] private BoxCollider collider;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("fallingHead"))
            {
                collider.enabled = false;

                foreach(Rigidbody rb in rigidbodies)
                {
                    rb.isKinematic = false;
                }
            }
        }
    }
}

