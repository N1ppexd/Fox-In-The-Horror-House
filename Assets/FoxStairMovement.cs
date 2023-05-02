using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox.Fox
{
    public class FoxStairMovement : MonoBehaviour
    {

        private Vector3 stairShootPoint;

        [SerializeField] private LayerMask whatIsStair; //mitkä layerit on portaita....??

        [SerializeField] private float maxDistance; //kuinka kauas ray menee maksimissaan...
        [SerializeField] private float stepSmooth;

        [SerializeField] private Rigidbody rb;


        [HideInInspector] public bool isStairZone;


        public void CheckForChairs()
        {
            stairShootPoint = transform.position;
            bool hitStair = Physics.Raycast(stairShootPoint, transform.forward, maxDistance, whatIsStair);

            //Jos portaisiin osutaan, mennään ylöspäin...
            if (hitStair)
            {
                rb.position += transform.up * stepSmooth;
            }
        }



        [SerializeField] private string stairZoneString; //xdd
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(stairZoneString))
            {
                isStairZone = true;
            }
        }
    }
}