using HorrorFox.Fox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HorrorFox.Enemies
{
    public class HunterAI : MonoBehaviour
    {

        [SerializeField] private NavMeshAgent agent;

        [SerializeField] Transform[] defaultTargetPoints;

        private Transform nextTargetPoint;//vihu menee t�h�n seuraavaksi....

        [Header("jos t�m� on true, vihu seuraa pelaajaa automaattisesti.... T�M� ON VAIN TESTAAMISTA VARTEN")]
        [SerializeField]private bool isChasingPlayer; //xddd

        [SerializeField] private Transform player;

        [Header("scripti, jossa on fov, jonka avulla tulevaisuudessa vihu etsii pelaajaa...")]
        [SerializeField] private HunterFov hunterFov; //huntterin n�k�kentt�...

        [Header("vihun animator homma...")]
        [SerializeField] private Animator animator;
        [SerializeField] private string walkStringAnimator;


        [Header("true, kun vihu etsii pelaajaa...")]
        [SerializeField] bool seekPlayer; // jos t�m� on false, ei edes yritet� etsi� pelaajaa.,........

        [Space(20)]
        [Header("vihun askelten ��net loop...")]
        [SerializeField] private AudioSource footStepAudio;

        [Space(20)]
        [Header("t�m� on true, kun vihju l�htee huoneesta...")]
        [SerializeField] bool isLeaving; // t�m� on true, kun l�hdet��n huoneesta....

        [Space(20)]
        [Header("Mill� et�isyydell� vihu grabaa pelaajan")]
        [SerializeField] private float grabDistance = 1.0f; //perus distance on 1...

        private void Awake()
        {
            agent.avoidancePriority = Random.Range(1, 100);

            nextTargetPoint = defaultTargetPoints[0];
            if (isChasingPlayer)
                nextTargetPoint = player;

            if(agent.isOnNavMesh)
                agent.SetDestination(nextTargetPoint.position);


            animator.Play(walkStringAnimator); // tehd��n k�velyAnimaatio...

        }

        // Update is called once per frame
        void Update()
        {
            if (!agent.isOnNavMesh)
                return;



            //T�SS� SIMPPELI KOODI JONK APIT�ISI TOIMIA. PARANNA HUOMENNA.
            if (hunterFov.isSeen && seekPlayer)
            {
                isChasingPlayer = true;
            }
            else if(!hunterFov.isSeen && seekPlayer)
            {
                isChasingPlayer = false;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName(walkStringAnimator) && !footStepAudio.isPlaying)
                footStepAudio.Play();
            else if (!animator.GetCurrentAnimatorStateInfo(0).IsName(walkStringAnimator))
                footStepAudio.Stop();


            if (nextTargetPoint == null || isLeaving)
                return;


            if (isChasingPlayer)
            {
                agent.SetDestination(nextTargetPoint.position);

                float sqrtDistance = Vector3.SqrMagnitude(transform.position - nextTargetPoint.position);

                if(sqrtDistance <= grabDistance * grabDistance)
                {
                    GrabPlayer(nextTargetPoint);
                }
            }
                

            

            if(transform.position - transform.up == agent.pathEndPosition || Vector3.Distance(transform.position -transform.up, agent.pathEndPosition) < 1f)
            {
                for(int i = 0; i < defaultTargetPoints.Length; i++)
                {
                    if(nextTargetPoint == defaultTargetPoints[i])
                    {
                        if (i + 1 > defaultTargetPoints.Length - 1)
                            nextTargetPoint = defaultTargetPoints[0];
                        else
                            nextTargetPoint = defaultTargetPoints[++i];
                        agent.SetDestination(nextTargetPoint.position);
                    }
                }
            }


            
        }


        private void ChasePlayer()
        {
            isChasingPlayer = true;
        }


        /// <summary>
        /// Spawning the hunter at the same position as <paramref name="point"/> transform.
        /// </summary>
        /// <param name="point"></param>
        public void SpawnHunter(Transform point)
        {
            agent.enabled = false;

            transform.position = point.position;

            agent.enabled = true;

            isLeaving = false;
        }

        /// <summary>
        /// This is played, when hunter leaves the room. Hunter moves to <paramref name="point"/> position..
        /// </summary>
        /// <param name="point"></param>
        public void LeaveRoom(Transform point)
        {
            isLeaving = true;

            agent.SetDestination(point.position);


        }




        /// <summary>
        /// t�m� on simppeli alku!!!
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("fox"))
            {
                GrabPlayer(collision.transform);
            }
        }



        /// <summary>
        /// T�M� ON TESTAUS, MUTTA SE ON SE JUTTU, KUN PELAAJA GRABATAAN.... PARANNETAAN MY�HEMMIN....
        /// </summary>
        private void GrabPlayer(Transform player)
        {
            player.gameObject.GetComponent<FoxHealth>().KillFox();
        }

    }

}

