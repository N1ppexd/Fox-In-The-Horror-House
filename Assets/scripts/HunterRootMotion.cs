using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox.Enemies.Animation
{
    public class HunterRootMotion : MonoBehaviour
    {
        [SerializeField] private Animator hunterAnimator;
        [SerializeField] private Transform hunterParent;

        private void Update()
        {
            //hunterParent.transform.position += hunterAnimator.rootPosition;
        }
    }
}

