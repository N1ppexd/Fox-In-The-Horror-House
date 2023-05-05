using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HorrorFox.Fox
{
    public class FoxHealth : MonoBehaviour
    {

        [SerializeField] private float maxHp;
        [HideInInspector] public float hp;

        // Start is called before the first frame update
        void Start()
        {
            hp = maxHp;
        }

        // Update is called once per frame
        void Update()
        {
            if (hp <= 0)
            {
                Debug.Log("fox should die now");
            }
        }
    }
}

