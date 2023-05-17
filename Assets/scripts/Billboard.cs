using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox
{
    public class Billboard : MonoBehaviour
    {
        private Vector3 lookPos;

        private Transform cam;

        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            lookPos = transform.position - (cam.position - transform.position).normalized;
            transform.LookAt(lookPos);
        }
    }
}

