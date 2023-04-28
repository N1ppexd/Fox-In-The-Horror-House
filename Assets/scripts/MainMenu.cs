using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace HorrorFox.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame() //kun painetaan nappia, tehd‰‰n t‰m‰. T‰st‰ aloitetaan peli...
        {
            //Debug.Log(SceneName + " on seuravaa scene johon loadataan nyt heti paikalla!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // ladataan seuraava scenee buioldIndexissˆ‰‰
        }
        public void QuitGame()  //kun painetaan "Quit game" nappia, poistutaan pelist‰
        {
            Application.Quit();
        }
    }
}

