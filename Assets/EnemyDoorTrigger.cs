using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HorrorFox
{
    public class EnemyDoorTrigger : MonoBehaviour
    {

        [SerializeField] private Animator doorAnimator;

        [SerializeField] private string doorOpenString, doorCloseString;

        public enum DoorMode
        {
            waitToOpen,
            waitToClose
        }

        public DoorMode doorMode;

        private void Start()
        {
            doorMode = DoorMode.waitToOpen;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("hunter"))
            {
                Debug.Log("trigger hunter door");

                if (doorMode == DoorMode.waitToOpen)
                {
                    OpenDoor();
                }

                else if (doorMode == DoorMode.waitToClose)
                {
                    CloseDoor();
                }
            }
        }

        IEnumerator waitForDoorClose()
        {
            yield return new WaitForSeconds(1);

            CloseDoor();
        }

        /// <summary>
        /// avataan ovi
        /// </summary>
        public void OpenDoor()
        {
            doorAnimator.Play(doorOpenString);
        }


        /// <summary>
        /// suljetaan ovi
        /// </summary>
        void CloseDoor()
        {
            doorAnimator.Play(doorCloseString);
        }
    }
}

