using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox
{
    public class GameManager : MonoBehaviour
    {
        public bool isPaused;

        public static GameManager instance;

        private void Awake()
        {
            instance = this;//t�lleen nopeeta ku ei t�ss� pit�s olla ongelmaa anytte
        }
    }
}

