using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HorrorFox
{
    public class KeyCollectable : MonoBehaviour
    {

        [SerializeField] private ParticleSystem takeKeyParticle;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("fox"))
            {
                takeKeyParticle.Play();
            }
        }
    }
}

