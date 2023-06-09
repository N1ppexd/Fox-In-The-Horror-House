using HorrorFox.Fox.Keys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox
{
    public class Door : MonoBehaviour
    {

        [SerializeField] private FoxInteract foxInteract;

        [SerializeField] Animator doorAnim;
        [SerializeField] string doorOpenAnimName;

        [SerializeField] private GameObject doorTrigger;//t�m� laitetaan pois sitten kun siihen on osuttu..


        [SerializeField] private AudioSource doorOpen;

        public void OpenDoor()
        {
            doorOpen.Play();
            doorTrigger.SetActive(false);
            doorAnim.Play(doorOpenAnimName);
        }
    }
}

