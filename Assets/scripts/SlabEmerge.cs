using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox
{
    public class SlabEmerge : MonoBehaviour
    {

        [SerializeField] private Animator slabAnimator;

        [SerializeField] private InteractObj interactObj;

        [SerializeField] private string moveSlabString;

        private void Awake()
        {
            interactObj.OnInteractHappened += InteractHappened;
        }

        private void InteractHappened()
        {
            slabAnimator.Play(moveSlabString);
        }
    }
}

