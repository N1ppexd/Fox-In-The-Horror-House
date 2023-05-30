using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox
{
    public class BisonHead : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        [SerializeField] private InteractObj interactObj;

        private void Awake()
        {
            interactObj.OnInteractHappened += InteractHappened;
        }

        private void InteractHappened()
        {
            rb.isKinematic = false;
        }
    }
}

