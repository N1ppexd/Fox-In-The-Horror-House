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


        private float
            currentClockTime,       //tämänhetkinen kellon aika
            currentHunterTime;      //tämän hetkinen hunterin aika (kuinka kauan se on ollut huoneessa..)


        bool hunterInRoom;



        [Space(30)]
        [SerializeField]
        private HunterAI[] hunterAI;


        // Start is called before the first frame update
        void Start()
        {
            currentClockTime = clockTimeDuration;
        }

        // Update is called once per frame
        void Update()
        {

            if (!hunterInRoom)
            {
                if (currentClockTime > 0)
                {
                    currentClockTime -= Time.deltaTime; //VÄHENNETÄÄN AIKAA
                }
                if (currentClockTime <= 0)
                {
                    SpawnHunters();
                }
            }
            else if (hunterInRoom)
            {
                if (currentClockTime > 0)
                {
                    currentClockTime -= Time.deltaTime; //VÄHENNETÄÄN AIKAA
                }
                if (currentClockTime <= 0)
                {
                    HuntersLeave();
                }
            }
            
        }



        private HunterAI currentHunter;

        private void SpawnHunters()
        {
            hunterInRoom = true;
            currentClockTime = clockTimeDuration; //resetataan clockTime;

            HunterAI hunter = hunterAI[Random.Range(0, hunterAI.Length)]; //Random rangessa toinen luku on aina poissasuljettu, eli ottaa random rangen 0 - hunterAI.length-1 väliltä...

            hunter.gameObject.SetActive(true);

            hunter.SpawnHunter(spawnPoints[Random.Range(0, spawnPoints.Length)]); //Spawnataan huntteri..

            currentHunter = hunter;
        }

        private void HuntersLeave()
        {

            hunterInRoom = false;

            currentHunter.LeaveRoom(spawnPoints[0]); //hunter lähtee huoneesta pois...

        }
    }
}
