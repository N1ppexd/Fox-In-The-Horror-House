using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HorrorFox
{
    public class SkipIntro : MonoBehaviour
    {
        bool animationPlayed = false;
        Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void IntroAnimationPlayed()
        {
            animationPlayed = true;
        }

        void Update()
        {
            if(Input.anyKey)
            {
                if(animationPlayed)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //ladataan seuraava scene...

                }
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f)
                {
                    anim.Play(anim.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 0.9f);
                }
            }

        }
    }
}

