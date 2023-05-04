using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HorrorFox.Enemies;

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

                    Kellokaappi.Instance.KelloMove(Quaternion.Euler(0, 0, pikkuViisariAngle));

                    currentClockTime -= Time.deltaTime; //V�HENNET��N AIKAA
                }
                if (currentClockTime <= 0)
                {
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
            Kellokaappi.Instance.MoveIsoViisari(Quaternion.Euler(0, 0, isoViisariAngle));
            
        }



        private HunterAI currentHunter;

        private void SpawnHunters()
        {
            hunterInRoom = true;
            currentClockTime = clockTimeDuration; //resetataan clockTime;

            currentHunterTime = hunterTimeDuration;

            HunterAI hunter = hunterAI[Random.Range(0, hunterAI.Length)]; //Random rangessa toinen luku on aina poissasuljettu, eli ottaa random rangen 0 - hunterAI.length-1 v�lilt�...

            hunter.gameObject.SetActive(true);

            hunter.SpawnHunter(spawnPoints[Random.Range(0, spawnPoints.Length)]); //Spawnataan huntteri..

            currentHunter = hunter;
        }

        private void HuntersLeave()
        {
            hunterInRoom = false;

            currentHunterTime = hunterTimeDuration;

            currentHunter.LeaveRoom(spawnPoints[0]); //hunter l�htee huoneesta pois...

        }
    }
}
