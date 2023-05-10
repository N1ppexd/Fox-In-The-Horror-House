using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox
{
    public class InteractObj : MonoBehaviour
    {
        public delegate void FunctionDelegate();

        public event FunctionDelegate OnInteractHappened; //kun kettu interaktaa t‰m‰n kanssa, tehd‰‰n t‰m‰...

        
        public void Interact()
        {
            Debug.Log("interacting");
            OnInteractHappened?.Invoke();
        }

    }
}