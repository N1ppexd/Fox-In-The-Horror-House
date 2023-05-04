using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HorrorFox
{
    public class Kellokaappi : MonoBehaviour
    {
        public static Kellokaappi Instance;

        [SerializeField] private Transform pikkuviisari, isoviisari;

        [SerializeField] private ParticleSystem explosionParticle; //explosionParticle tulee, kun lyhyt viisari menee yl�s.


        [SerializeField] private AudioSource clockAudio;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else if(Instance != null)
                Destroy(Instance);
        }


        /// <summary>
        /// Kello liikuttaa pikkuviisaria <paramref name="viisariRotation"/> local-rotaatiolla
        /// </summary>
        /// <param name="viisariRotation"></param>
        public void KelloMove(Quaternion viisariRotation)
        {
            pikkuviisari.localRotation = viisariRotation;
        }


        /// <summary>
        /// Kello r�j�ht��, kun se lopettaa py�rimisen..
        /// </summary>
        public void KelloStop()
        {
            clockAudio.Play();

            pikkuviisari.localRotation = Quaternion.Euler(0, 0, 0);

            explosionParticle.Play();
        }


        /// <summary>
        /// Liikuttaa isoa viisaria, joka n�ytt�� levelin ajan <paramref name="viisariRotation"/> local-rotaatiolla...
        /// </summary>
        /// <param name="viisariRotation"></param>
        public void MoveIsoViisari(Quaternion viisariRotation)
        {
            isoviisari.localRotation = viisariRotation;
        }
    }
}

