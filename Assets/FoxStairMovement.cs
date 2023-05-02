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


        public bool isStairZone;


        private Transform stairTrigger;

        [SerializeField] private Transform bodyTransform;

        public void CheckForChairs()
        {
            if (!isStairZone)
                return;

            stairShootPoint = transform.position;
            bool hitStair = Physics.Raycast(stairShootPoint, transform.forward, maxDistance, whatIsStair);

            //Jos portaisiin osutaan, mennään ylöspäin...
            if (hitStair)
            {
                rb.position += Vector3.up * stepSmooth;
            }

            /*Vector3 rotationAxis = Vector3.Cross(transform.forward, stairTrigger.up);

            bodyTransform.rotation = Quaternion.Euler(rotationAxis * 45);

            bodyTransform.Rotate(bodyTransform.up * 90);

            RaycastHit hit;

            Physics.Raycast(transform.position, -transform.up, out hit, maxDistance, whatIsStair);

            bodyTransform.rotation = Quaternion.FromToRotation(transform.up, hit.normal);*/

            //bodyTransform.rotation = 
        }



        [SerializeField] private string stairZoneString; //xdd
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(stairZoneString))
            {

                rb.mass = 10;

                stairTrigger = other.transform;
                isStairZone = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(stairZoneString))
            {
                rb.mass = 1;

                bodyTransform.rotation = Quaternion.Euler(0,0,0);

                isStairZone = false;
            }
        }
    }
}