using HorrorFox.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HorrorFox
{
    public class HunterAnimationEvents : MonoBehaviour
    {

        [SerializeField] private HunterFov hunterFov;
        [SerializeField] private GameObject fox;


        private bool hasBeenGrabbed;

        public void GrabFox()
        {
            hunterFov.foxMovement.isStopped = true;


        }

    }
}

