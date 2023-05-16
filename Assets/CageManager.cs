using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox
{
    public class CageManager : MonoBehaviour
    {
        [SerializeField] private GameObject doorCollider;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("floor"))
            {
                OpenDoor(); //kun ostaan maahan, ovi avataan... t‰m‰n j‰lkeen h‰kist‰ muutetaan draggableObj...
            }
        }


        private void OpenDoor()
        {
            doorCollider.SetActive(false); //tehd‰‰n nyt aluksi vain n‰in, koska pit‰‰ testata...
        }

    }
}
