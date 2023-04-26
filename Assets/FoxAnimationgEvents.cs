using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HorrorFox.Fox.Animation
{
    public class FoxAnimationgEvents : MonoBehaviour
    {

        [SerializeField] private FoxMovement foxMovement;
        public void StopCrouching()
        {
            foxMovement.StopSquashing();    //tehd‰‰n n‰in...
        }
    }
}

