using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HorrorFox
{
    public class IntroSound : MonoBehaviour
    {

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if(SceneManager.GetActiveScene().buildIndex >= 4 || SceneManager.GetActiveScene().buildIndex == 0)
            {
                Destroy(gameObject);
            }
        }
    }

}
