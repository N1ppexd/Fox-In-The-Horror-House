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

        [SerializeField]private bool isChasingPlayer; //xddd

        [SerializeField] private Transform player;


        [SerializeField] private HunterFov hunterFov; //huntterin n�k�kentt�...

        [SerializeField] private Animator animator;
        [SerializeField] private string walkStringAnimator;

        [SerializeField] bool seekPlayer; // jos t�m� on false, ei edes yritet� etsi� pelaajaa.,........

        [SerializeField] private AudioSource footStepAudio;

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


            if(isChasingPlayer)
                agent.SetDestination(nextTargetPoint.position);

            if (nextTargetPoint == null || !isChasingPlayer)
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


            if (animator.GetCurrentAnimatorStateInfo(0).IsName(walkStringAnimator) && !footStepAudio.isPlaying)
                footStepAudio.Play();
            else if (!animator.GetCurrentAnimatorStateInfo(0).IsName(walkStringAnimator))
                footStepAudio.Stop();
        }


        private void ChasePlayer()
        {
            isChasingPlayer = true;
        }

    }

}

