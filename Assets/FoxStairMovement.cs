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

        public void CheckForChairs()
        {
            if (!isStairZone)
                return;

            stairShootPoint = transform.position;
            bool hitStair = Physics.Raycast(stairShootPoint, transform.forward, maxDistance, whatIsStair);

            //Jos portaisiin osutaan, mennään ylöspäin...
            if (hitStair)
            {
                rb.position += transform.up * stepSmooth;
            }

            Vector3 rotationAngle = Vector3.Cross(transform.right, stairTrigger.transform.up);
            rotationAngle = new Vector3(rotationAngle.x, transform.rotation.y, rotationAngle.z);
            transform.localRotation = Quaternion.Euler(rotationAngle * 45);
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

                isStairZone = false;
            }
        }
    }
}