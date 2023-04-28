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

        private void Awake()
        {
            agent.avoidancePriority = Random.Range(1, 100);

            nextTargetPoint = defaultTargetPoints[0];
            if (isChasingPlayer)
                nextTargetPoint = player;
            agent.SetDestination(nextTargetPoint.position);
            

        }

        // Update is called once per frame
        void Update()
        {

            //TÄSSÄ SIMPPELI KOODI JONK APITÄISI TOIMIA. PARANNA HUOMENNA.
            if (hunterFov.isSeen)
            {
                isChasingPlayer = true;
            }
            else
            {
                isChasingPlayer = false;
            }


            if(isChasingPlayer)
                agent.SetDestination(nextTargetPoint.position);

            if (nextTargetPoint == null || !isChasingPlayer)
                return;

            if(transform.position == agent.pathEndPosition)
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

    }

}

