using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HorrorFox
{
    public class EnemyDoorTrigger : MonoBehaviour
    {

        [SerializeField] private Animator doorAnimator;

        [SerializeField] private string doorOpenString, doorCloseString;

        [SerializeField] private AudioSource doorSmashAudio;

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

        private GameObject hunter;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("hunter"))
            {
                hunter = other.gameObject;

                Debug.Log("trigger hunter door");

                if (doorMode == DoorMode.waitToOpen)
                {
                    OpenDoor();
                }

                else if (doorMode == DoorMode.waitToClose)
                {
                    StartCoroutine(waitForDoorClose());
                }
            }
        }

        IEnumerator waitForDoorClose()
        {
            yield return new WaitForSeconds(1);

            CloseDoor();

            yield return new WaitForSeconds(0.5f);
            hunter.transform.parent.gameObject.SetActive(false);
        }

        /// <summary>
        /// avataan ovi
        /// </summary>
        public void OpenDoor()
        {
            if (doorAnimator == null)
                return;

            doorSmashAudio.Play();
            doorAnimator.Play(doorOpenString);
        }


        /// <summary>
        /// suljetaan ovi
        /// </summary>
        void CloseDoor()
        {
            if (doorAnimator == null)
                return;

            doorSmashAudio.Play();
            doorAnimator.Play(doorCloseString);
        }
    }
}

