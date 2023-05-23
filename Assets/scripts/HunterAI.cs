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

        private Transform nextTargetPoint;//vihu menee tähän seuraavaksi....

        [Header("jos tämä on true, vihu seuraa pelaajaa automaattisesti.... TÄMÄ ON VAIN TESTAAMISTA VARTEN")]
        [SerializeField]private bool isChasingPlayer; //xddd

        [SerializeField] private Transform player;

        [Header("scripti, jossa on fov, jonka avulla tulevaisuudessa vihu etsii pelaajaa...")]
        [SerializeField] private HunterFov hunterFov; //huntterin näkökenttä...

        [Header("vihun animator homma...")]
        [SerializeField] private Animator animator;
        [SerializeField] private string walkStringAnimator, grabFoxStringAnimator;


        [Header("true, kun vihu etsii pelaajaa...")]
        [SerializeField] bool seekPlayer; // jos tämä on false, ei edes yritetä etsiä pelaajaa.,........

        [Space(20)]
        [Header("vihun askelten äänet loop...")]
        [SerializeField] private AudioSource footStepAudio;

        [Space(20)]
        [Header("tämä on true, kun vihju lähtee huoneesta...")]
        public bool isLeaving; // tämä on true, kun lähdetään huoneesta....

        [Space(20)]
        [Header("Millä etäisyydellä vihu grabaa pelaajan")]
        [SerializeField] private float grabDistance = 1.0f; //perus distance on 1...

        private void Awake()
        {
            //agent.avoidancePriority = Random.Range(1, 100);

            nextTargetPoint = defaultTargetPoints[0];
            if (isChasingPlayer)
                nextTargetPoint = player;

            if(agent.isOnNavMesh)
                agent.SetDestination(nextTargetPoint.position);

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
                Debug.Log("nähdään pelaaja");
                isChasingPlayer = true;
                nextTargetPoint = player;
            }
            else if(!hunterFov.isSeen && seekPlayer)
            {
                Debug.Log("nextTargetPoint position = " + nextTargetPoint.position);
                isChasingPlayer = false;

                
                if (nextTargetPoint == player)
                    nextTargetPoint = defaultTargetPoints[Random.Range(0,1)];
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName(walkStringAnimator) && !footStepAudio.isPlaying)
                footStepAudio.Play();
            else if (!animator.GetCurrentAnimatorStateInfo(0).IsName(walkStringAnimator))
                footStepAudio.Stop();


            if (nextTargetPoint == null || isLeaving)
                return;

            agent.SetDestination(nextTargetPoint.position);


            if (isChasingPlayer)
            {

                float sqrtDistance = Vector3.SqrMagnitude(transform.position - nextTargetPoint.position);

                if(sqrtDistance <= grabDistance * grabDistance)
                {
                    GrabPlayer(nextTargetPoint);
                }

                return;
            }

            if(transform.position == nextTargetPoint.position || Vector3.Distance(transform.position, nextTargetPoint.position) < 0.1f)
            {
                Debug.Log("yritetään ottaa uusi nextTargetposition...");
                agent.SetDestination(transform.position);

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
        /// tämä on simppeli alku!!!
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
        /// TÄMÄ ON TESTAUS, MUTTA SE ON SE JUTTU, KUN PELAAJA GRABATAAN.... PARANNETAAN MYÖHEMMIN....
        /// </summary>
        private void GrabPlayer(Transform player)
        {
            agent.SetDestination(transform.position);
            animator.Play(grabFoxStringAnimator);

            StartCoroutine(waitGrab());
        }


        //tää on väliaikainenhomma jonka poistan tulevaisuudessa.
        IEnumerator waitGrab()
        {
            yield return new WaitForSeconds(2);

            player.gameObject.GetComponent<FoxHealth>().KillFox();
        }

    }

}

