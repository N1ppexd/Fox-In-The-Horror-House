using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace HorrorFox
{
    public class Kellokaappi : MonoBehaviour
    {
        public static Kellokaappi Instance;

        [SerializeField] private Transform pikkuviisari, isoviisari, isoviisariUI, pikkuviisariUI;

        [SerializeField] private ParticleSystem explosionParticle; //explosionParticle tulee, kun lyhyt viisari menee ylös.


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
            isoviisari.localRotation = viisariRotation;
            isoviisariUI.localRotation = Quaternion.Inverse(viisariRotation);
        }


        /// <summary>
        /// Kello räjähtää, kun se lopettaa pyörimisen..
        /// </summary>
        public void KelloStop()
        {
            clockAudio.Play();

            isoviisari.localRotation = Quaternion.Euler(0, 0, 0);
            isoviisariUI.localRotation = Quaternion.Euler(0, 0, 0);

            explosionParticle.Play();
        }


        /// <summary>
        /// Liikuttaa isoa viisaria, joka näyttää levelin ajan <paramref name="viisariRotation"/> local-rotaatiolla...
        /// </summary>
        /// <param name="viisariRotation"></param>
        public void MoveIsoViisari(Quaternion viisariRotation)
        {
            pikkuviisari.localRotation = viisariRotation;
            pikkuviisariUI.localRotation = Quaternion.Inverse(viisariRotation);
        }
    }
}

