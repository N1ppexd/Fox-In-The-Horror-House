using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox
{
    public class BookStack : MonoBehaviour
    {
        [SerializeField] Rigidbody[] bookStackRbs;


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("hunter"))
            {
                foreach(Rigidbody book in bookStackRbs)
                {
                    book.isKinematic = false;
                }
            }
        }
    }
}

