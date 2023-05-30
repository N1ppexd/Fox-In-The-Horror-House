using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HorrorFox
{
    public class StartIntro : MonoBehaviour
    {
        
        public void CloseStartIntro()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //ladataan seuraava scene...
        }
    }
}

