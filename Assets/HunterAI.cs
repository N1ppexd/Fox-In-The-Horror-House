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

        private void Awake()
        {
            agent.avoidancePriority = Random.Range(1, 100);

            nextTargetPoint = defaultTargetPoints[0];

            agent.SetDestination(nextTargetPoint.position);
        }

        // Update is called once per frame
        void Update()
        {
            if (nextTargetPoint == null)
                return;

            if(transform.position == agent.pathEndPosition)
            {
                for(int i = 0; i < defaultTargetPoints.Length; i++)
                {
                    if(nextTargetPoint == defaultTargetPoints[i])
                    {
                        nextTargetPoint = defaultTargetPoints[++i];
                        agent.SetDestination(nextTargetPoint.position);
                    }
                }
            }
        }
    }
}

