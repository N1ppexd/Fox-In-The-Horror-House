using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


namespace HorrorFox.Enemies
{
    public class HunterFov : MonoBehaviour
    {

        [SerializeField] private float fovAngle;
        [SerializeField] private float seeDistance;
        [SerializeField] private float detectionWait;//kuinka kauan odotetaan, ett� ammutaan...

        private bool isSeen;

        [SerializeField] private LayerMask whatIsFox, whatIsObstacle;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public List<Transform> targets = new List<Transform>(); //targetit, jotka on vihollisen fovin sis�ll�.
        private void FOVCheck()
        {

            targets.Clear();//tyhjennet��n...
            Vector3 lookPositionVector = transform.position;
            Collider[] rangeChecks = Physics.OverlapSphere(lookPositionVector, seeDistance, whatIsFox);

            //float playerRotation = Vector3.Angle(enemyAxis, transform.forward);


            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 targetDir = target.position - lookPositionVector;

                Vector3 targetDirVector = targetDir.normalized; //en tied� tarvitaanko.... t�m� on suuntavektori ufoon p�in vihollisesta katsottuna....
                targetDirVector.y = 0;                          //laitetaan y nollaan... eli ei katsota yl�sp�in...




                if (Vector3.Angle(transform.forward, targetDirVector) < fovAngle / 2)//jatetaan kahdella, koska niin
                {

                    float distToTarget = Vector3.Distance(transform.position + targetDir, transform.position);
                    if (!Physics.Raycast(transform.position, targetDir, distToTarget, whatIsObstacle))//jos v�liss� ei ole esteit�
                    {
                        /*
                        if (!yellShock.isPlaying)
                            yellShock.Play();
                        */
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
                }
            }
            else
            {
                isSeen = false;
            }
        }

    }
}