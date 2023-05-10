using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HorrorFox.Fox
{
    public class FoxHealth : MonoBehaviour
    {

        [SerializeField] private float maxHp;
        [HideInInspector] public float hp;


        [SerializeField] private GameObject DeathScreen; //tulee p‰‰lle, kun kettu kuolee...

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
                KillFox();
                Debug.Log("fox should die now");
            }
        }


        //kettu kuolee
        public void KillFox()
        {
            DeathScreen.SetActive(true);
        }
    }
}

