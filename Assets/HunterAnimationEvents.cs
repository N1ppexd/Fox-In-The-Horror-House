using HorrorFox.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HorrorFox
{
    public class HunterAnimationEvents : MonoBehaviour
    {

        [SerializeField] private HunterFov hunterFov;
        [SerializeField] private HunterAI hunterAI;
        [SerializeField] private GameObject fox;

        [SerializeField] private Transform grabTransform;


        private bool hasBeenGrabbed;

        public void GrabFox()
        {
            hunterAI.hasBeenGrabbed = true;
            hunterFov.foxMovement.isStopped = true;

            hasBeenGrabbed = true;
        }


        private void Update()
        {
            if (hasBeenGrabbed)
            {
                fox.transform.position = grabTransform.position;
                fox.transform.rotation = grabTransform.rotation;

            }
        }

    }
}

