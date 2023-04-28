using HorrorFox.Fox;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace HorrorFox
{
    public class SceneChanger : MonoBehaviour
    {

        [SerializeField] private FoxMovement foxMovement;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("fox"))
            {
                ScreenFader.instance.FadeOut(); // ruutu menee mustaksi........

                //kun ruutu menee mustaksi, samalla odotetaan sekuntti ennen kuin menn‰‰n uuteen sceeneen, jotta ruudun fadeout animaatio on kerennyt menn‰ loppuun..
                StartCoroutine(WaitForSceneChange());

            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("fox"))
            {
                ScreenFader.instance.FadeOut(); // ruutu menee mustaksi........

                //kun ruutu menee mustaksi, samalla odotetaan sekuntti ennen kuin menn‰‰n uuteen sceeneen, jotta ruudun fadeout animaatio on kerennyt menn‰ loppuun..
                StartCoroutine(WaitForSceneChange());
            }
        }


        /// <summary>
        /// Odotetaan sekuntti, ja ladataan uusi scene....
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitForSceneChange()//jotaki paskaa t‰ss‰ n‰iun
        {
            foxMovement.movementMode = FoxMovement.MovementMode.transitioning;
            yield return new WaitForSeconds(1);


            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // ladataan seuraava scenee buioldIndexissˆ‰‰

        }


    }
}

