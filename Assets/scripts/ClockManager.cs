using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HorrorFox.Enemies;
using Cinemachine;

namespace HorrorFox.Clock
{
    public class ClockManager : MonoBehaviour
    {

        [Header("Yhden kellon syklin kesto")]
        [Range(1,120)]
        [SerializeField] float clockTimeDuration;

        [Space(20)]
        [Header("Kuinka kauan hunter on huoneessa")]
        [Range(1, 120)]
        [SerializeField] float hunterTimeDuration;


        [Space(20)]
        [Header("hunttereiden spawn pointit")]
        [SerializeField] private Transform[] spawnPoints;

        [Space(20)]
        [Header("max level pituus (minuuteissa)")]
        [Range(1,5)]
        [SerializeField] private float maxLevelDuration;


        private float
            currentClockTime,       //t�m�nhetkinen kellon aika
            currentHunterTime,      //t�m�n hetkinen hunterin aika (kuinka kauan se on ollut huoneessa..)
            currentLevelTime;


        bool hunterInRoom;


        [SerializeField] private Transform clockViisari; //viisarikelloon, joka py�rii...


        [Space(30)]
        [SerializeField]
        private HunterAI[] hunterAI;


        [Space(20)]
        [Header("OviSYsteemi, jotta avataan ovi")]
        [SerializeField] private EnemyDoorTrigger doorTrigger;


        [Space(20)]
        [Header("Pelaajan ameran cinemachine")]
        [SerializeField] private CinemachineVirtualCamera cineMachineCam;




        //private hommat kameran t�rin�n kontrollointia varten...
        private float
            startingIntensity,
            shakeTimer,
            shakeTimerTotal;



        // Start is called before the first frame update
        void Start()
        {
            currentClockTime = clockTimeDuration;

            currentLevelTime = maxLevelDuration * 60; //kerrotaaan 60:ll�, koska maxLevelDuration on minuuteissa...
        }


        float pikkuViisariAngle, isoViisariAngle;

        // Update is called once per frame
        void Update()
        {

            if (!hunterInRoom)
            {
                if (currentClockTime > 0)
                {
                    pikkuViisariAngle = currentClockTime / clockTimeDuration * 360;

                    Kellokaappi.Instance.KelloMove(Quaternion.Euler(0, 0, -pikkuViisariAngle));

                    currentClockTime -= Time.deltaTime; //V�HENNET��N AIKAA
                }
                if (currentClockTime <= 0)
                {
                    Shake(3, 2); //kameran pit�isi t�rist�...
                    Kellokaappi.Instance.KelloStop();
                    SpawnHunters();
                }
            }
            else if (hunterInRoom)
            {
                if (currentHunterTime > 0)
                {
                    currentHunterTime -= Time.deltaTime; //V�HENNET��N AIKAA
                }
                if (currentHunterTime <= 0)
                {
                    HuntersLeave();
                }
            }

            currentLevelTime -= Time.deltaTime;
            isoViisariAngle = currentLevelTime / (maxLevelDuration * 60) * 360;
            Kellokaappi.Instance.MoveIsoViisari(Quaternion.Euler(0, 0, -isoViisariAngle));



            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
                cineMachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain =
                    Mathf.Lerp(startingIntensity, 0, 1 - shakeTimer / shakeTimerTotal);

            }

        }



        private HunterAI currentHunter;

        private void SpawnHunters()
        {
            hunterInRoom = true;

            doorTrigger.doorMode = EnemyDoorTrigger.DoorMode.waitToOpen;

            currentClockTime = clockTimeDuration; //resetataan clockTime;

            currentHunterTime = hunterTimeDuration;

            HunterAI hunter = hunterAI[Random.Range(0, hunterAI.Length)]; //Random rangessa toinen luku on aina poissasuljettu, eli ottaa random rangen 0 - hunterAI.length-1 v�lilt�...

            hunter.gameObject.SetActive(true);

            hunter.SpawnHunter(spawnPoints[Random.Range(0, spawnPoints.Length)]); //Spawnataan huntteri..

            currentHunter = hunter;
        }

        private void HuntersLeave()
        {
            doorTrigger.doorMode = EnemyDoorTrigger.DoorMode.waitToClose;

            hunterInRoom = false;

            currentHunterTime = hunterTimeDuration;

            currentHunter.LeaveRoom(spawnPoints[0]); //hunter l�htee huoneesta pois...

        }


        //CAMERA SHAKE

        /// <summary>
        /// <paramref name="intensity"/> is the intensity of the camera shake while <paramref name="time"/> is the duration of the shake in seconds.
        /// </summary>
        /// <param name="intensity"></param>
        /// <param name="time"></param>
        public void Shake(float intensity, float time)//cinemachine kamera t�rin�
        {
            cineMachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
            startingIntensity = intensity;
            shakeTimer = time;
            shakeTimerTotal = time;
        }
    }
}
