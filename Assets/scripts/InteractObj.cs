using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorFox
{
    public class InteractObj : MonoBehaviour
    {

        [SerializeField] private AudioSource vipuSource;


        public delegate void FunctionDelegate();

        public event FunctionDelegate OnInteractHappened; //kun kettu interaktaa t‰m‰n kanssa, tehd‰‰n t‰m‰...

        
        public void Interact()
        {
            vipuSource.Play();
            Debug.Log("interacting");
            OnInteractHappened?.Invoke();
        }

    }
}