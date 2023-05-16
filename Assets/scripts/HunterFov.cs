using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using HorrorFox.Fox;
using UnityEngine.AI;

namespace HorrorFox.Enemies
{
    public class HunterFov : MonoBehaviour
    {

        [SerializeField] private float fovAngle;
        public float seeDistance;
        [SerializeField] private float detectionWait;//kuinka kauan odotetaan, ett� ammutaan...

        [HideInInspector] public bool isSeen;//true, kun hunter n�kee ketun.

        [SerializeField] private LayerMask whatIsFox, whatIsObstacle;


        [Space(10)]
        [Header("ketun movement scripti, jotta voidaan katsoa, onko se piilossa")]
        [SerializeField] FoxMovement foxMovement; //voidaan sitten, kun olen laittanut statemachinen, niin ett� katsotaan, onko se idle vai liikku t�m�n lis�ksi, jolloin se huomataan, jos ei ole idle...
        

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(CheckTheFov());
        }

        // Update is called once per frame
        void Update()
        {

        }


        IEnumerator CheckTheFov()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                FOVCheck();

            }
        }



        public List<Transform> targets = new List<Transform>(); //targetit, jotka on vihollisen fovin sis�ll�.
        private void FOVCheck()
        {
            targets.Clear();//tyhjennet��n...
            Vector3 lookPositionVector = transform.position;
            Collider[] rangeChecks = Physics.OverlapSphere(lookPositionVector, seeDistance, whatIsFox);

            //float playerRotation = Vector3.Angle(enemyAxis, transform.forward);

            Debug.Log("foxMovement.isHiding = " + foxMovement.isHiding);

            Debug.Log("foxMovement mode = " + foxMovement.movementMode);

            if (foxMovement.isHiding && foxMovement.movementMode == FoxMovement.MovementMode.idle)//jos on paikallaan ja piilossa...
            {
                GameObject[] draggableObjects = GameObject.FindGameObjectsWithTag("draggableObj");

                foreach (GameObject draggableObject in draggableObjects)
                    draggableObject.GetComponent<NavMeshObstacle>().enabled = true;

                isSeen = false;

                Debug.Log("vihollinen ei voi n�hd� sinua...");
                return;
            }
            else
            {
                GameObject[] draggableObjects = GameObject.FindGameObjectsWithTag("draggableObj");

                foreach (GameObject draggableObject in draggableObjects)
                    draggableObject.GetComponent<NavMeshObstacle>().enabled = true;
            }
                

            
            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 targetDir = target.position - lookPositionVector;

                Vector3 targetDirVector = targetDir.normalized; //en tied� tarvitaanko.... t�m� on suuntavektori ufoon p�in vihollisesta katsottuna....
                targetDirVector.y = 0;                          //laitetaan y nollaan... eli ei katsota yl�sp�in...

                isSeen = true; //t�m� on v�liaikainen juttu....
                /*
                if (Vector3.Angle(transform.forward, targetDirVector) < fovAngle / 2)//jatetaan kahdella, koska niin
                {

                    float distToTarget = Vector3.Distance(transform.position + targetDir, transform.position);
                    if (!Physics.Raycast(transform.position, targetDir, distToTarget, whatIsObstacle))//jos v�liss� ei ole esteit�
                    {*/
                        /*
                        if (!yellShock.isPlaying)
                            yellShock.Play();
                        *//*
                        targets.Add(target);//lis�t��n kettu listaan..
                        isSeen = true;

                        //enemyAnim.Play(surprisedAnim);
                    }
                    else
                    {
                        isSeen = false;
                    }
                }
                else
                {

                    isSeen = false;
                }*/
            }
            else
            {
                isSeen = false;
            }
        }

    }
}