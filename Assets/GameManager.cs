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
            instance = this;//tälleen nopeeta ku ei tässä pitäs olla ongelmaa anytte
        }
    }
}

