using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace HorrorFox.UI
{
    public class UiButtons : MonoBehaviour
    {

        [SerializeField] private GameObject pauseScreen;


        private InputMaster inputMaster;


        private void OnEnable()
        {
            inputMaster = new InputMaster();

            inputMaster.Enable();

            inputMaster.UI.Pause.performed += _ => PauseGame();
        }

        private void OnDisable()
        {


            inputMaster.UI.Pause.performed -= _ => PauseGame();

            inputMaster.Disable();
        }


        // POISTUTAAN KOKO PELISTÄÖ
        public void ExitGame()
        {
            Application.Quit(); //xddd
        }

        // MENNÄÄN MAINMENUUN
        public void MainMenu()
        {
            SceneManager.LoadScene(0); //tälleen noppeeta vaan nytte.
        }


        private void PauseGame()
        {
            Debug.Log("PAUSEGAME!!"); //toimis nyt

            GameManager.instance.isPaused = true;

            Time.timeScale = 0;

            pauseScreen.SetActive(true);
        }

        // JATKETAAN PELIÄ...
        public void ResumeGame()
        {
            GameManager.instance.isPaused = false;
            Time.timeScale = 1;           //aika menee takaisin normaaliksi...
            pauseScreen.SetActive(false); // laitetaan pausemenu pois päältä
        }

    }
}

