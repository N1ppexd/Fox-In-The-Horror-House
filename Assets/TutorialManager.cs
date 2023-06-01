using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox
{
    public class TutorialManager : MonoBehaviour
    {

        [SerializeField] private Transform tutorialSpawn, normalSpawn;

        [SerializeField] private Transform fox;

        private void Awake()
        {
            if(PlayerPrefs.GetInt("tutorialCompleted") == 0)
            {
                fox.position = tutorialSpawn.position;
            }
            else
            {
                fox.position = normalSpawn.position;
            }
        }



        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("fox"))
            {
                PlayerPrefs.SetInt("tutorialCompleted", PlayerPrefs.GetInt("tutorialCompleted") + 1);
            }
        }
    }
}

