using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{

    [SerializeField] Animator screenFadeAnimator;//animaattori...

    public static ScreenFader instance;


    public string fadeInString, fadeOutString; //fadeIn on kun musta menee pois, fadeOut on kun musta tulee ruudulle...


    private void Awake()
    {
        if (ScreenFader.instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        FadeIn();//musta ruutu menee ei mustaksi..................................
    }


    public void FadeIn()
    {
        screenFadeAnimator.Play(fadeInString);
    }

    public void FadeOut()
    {
        screenFadeAnimator.Play(fadeOutString);
    }

}
