using HorrorFox.Enemies;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HorrorFox
{
    public class HunterAnimationEvents : MonoBehaviour
    {

        [SerializeField] private HunterFov hunterFov;
        [SerializeField] private HunterAI hunterAI;
        [SerializeField] private GameObject fox;

        [SerializeField] private Transform grabTransform;


        private bool hasBeenGrabbed;

        [SerializeField] private float grabFoxDistance;

        public void GrabFox()
        {
            if ((fox.transform.position - grabTransform.position).sqrMagnitude > grabFoxDistance * grabFoxDistance)
                return;

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

                StartCoroutine(RestartScene());   
            }
        }


        IEnumerator RestartScene()
        {
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}

