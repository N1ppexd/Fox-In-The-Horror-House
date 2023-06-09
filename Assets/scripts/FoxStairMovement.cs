using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox.Fox
{
    public class FoxStairMovement : MonoBehaviour
    {

        private Vector3 stairShootPoint;

        [SerializeField] private LayerMask whatIsStair; //mitk� layerit on portaita....??

        [SerializeField] private float maxDistance; //kuinka kauas ray menee maksimissaan...
        [SerializeField] private float stepSmooth;

        [SerializeField] private Rigidbody rb;


        public bool isStairZone;


        private Transform stairTrigger;

        [SerializeField] private Transform bodyTransform;

        public void CheckForChairs()
        {
            if (!isStairZone)
            {
                //rb.isKinematic = false;
                rb.freezeRotation = true;
                return;
            }
            //rb.isKinematic = true;   

            stairShootPoint = transform.position;
            bool hitStair = Physics.Raycast(stairShootPoint, transform.forward, maxDistance, whatIsStair);

            //Jos portaisiin osutaan, menn��n yl�sp�in...
            if (hitStair)
            {
                rb.position += Vector3.up * stepSmooth;
            }
        }


        [SerializeField] private string stairZoneString; //xdd
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(stairZoneString))
            {

                stairTrigger = other.transform;
                isStairZone = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(stairZoneString))
            {
                bodyTransform.rotation = Quaternion.Euler(0,0,0);

                isStairZone = false;
            }
        }
    }
}