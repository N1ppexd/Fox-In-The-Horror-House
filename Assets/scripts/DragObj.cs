using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox
{
    public class DragObj : MonoBehaviour
    {

        public bool isBeingDragged;
        public Transform draggerObj; //tämä raahaa esinettä...

        private Vector3 offsetVector;

        [SerializeField] ConfigurableJoint joint;

        // Update is called once per frame
        void Update()
        {
            if (isBeingDragged)
            {
                joint.connectedAnchor = draggerObj.root.position;

                /*
                transform.position = draggerObj.position + offsetVector;
                transform.rotation = draggerObj.root.rotation;*/
            }
        }

        public void SetDragOffset()
        {
            offsetVector = transform.position - draggerObj.position;
        }
    }
}

