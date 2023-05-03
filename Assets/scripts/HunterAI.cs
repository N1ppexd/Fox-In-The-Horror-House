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

        private Transform nextTargetPoint;//vihu menee tähän seuraavaksi....

        [SerializeField]private bool isChasingPlayer; //xddd

        [SerializeField] private Transform player;


        [SerializeField] private HunterFov hunterFov; //huntterin näkökenttä...

        [SerializeField] private Animator animator;
        [SerializeField] private string walkStringAnimator;

        [SerializeField] bool seekPlayer; // jos tämä on false, ei edes yritetä etsiä pelaajaa.,........

        [SerializeField] private AudioSource footStepAudio;

        [SerializeField] bool isLeaving; // tämä on true, kun lähdetään huoneesta....

        private void Awake()
        {
            agent.avoidancePriority = Random.Range(1, 100);

            nextTargetPoint = defaultTargetPoints[0];
            if (isChasingPlayer)
                nextTargetPoint = player;

            if(agent.isOnNavMesh)
                agent.SetDestination(nextTargetPoint.position);


            animator.Play(walkStringAnimator); // tehdään kävelyAnimaatio...

        }

        // Update is called once per frame
        void Update()
        {
            if (!agent.isOnNavMesh)
                return;



            //TÄSSÄ SIMPPELI KOODI JONK APITÄISI TOIMIA. PARANNA HUOMENNA.
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


            if (isChasingPlayer)
                agent.SetDestination(nextTargetPoint.position);

            if (nextTargetPoint == null || !isChasingPlayer || isLeaving)
                return;

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

    }

}

