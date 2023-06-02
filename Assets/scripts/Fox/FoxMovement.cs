using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using HorrorFox.Enemies;
using TMPro;

namespace HorrorFox.Fox
{
    public class FoxMovement : MonoBehaviour
    {
        public InputMaster inputMaster;

        [SerializeField] HunterFov hunterFov; 


        [SerializeField] Rigidbody rb; //rigidbody ketulle...
        [SerializeField] private float speed, jumpForce, runSpeed; //speed on liikkuminen, jump force on hyppy...

        [SerializeField] private Animator foxAnimator;
        [SerializeField] private string squashStartStrin, squashEndString, walk, run, jump, idle, land, fly;

        [SerializeField] private Transform body;


        [HideInInspector] public bool isHiding;
        [HideInInspector] public bool isSeen; //kun kettu n‰hd‰‰n...
        [HideInInspector] public bool isStopped;
        [HideInInspector] public bool isJumping;//Isjumping on true, kun painetaan hyppy nappia pohjassa.

        private bool waitToSquashBackUp; //isUnderbed on kun ollaan s‰ngyn alla, waitTOSquashBackUp on kun ei paineta squash nappia kun ollaan s‰ngyn alla

        private bool isGrounded;//kun kettu on maassa

        private bool isSquashing;

        [Space(10)]
        [Header("nopeus, jolla kettu k‰‰ntyy")]
        [SerializeField] private float turnSpeed, fasterTurnSpeed = 15; //nopeus, jolla kettu k‰‰ntyy

        [Space(10)]
        [Header("maksimi m‰‰r‰ sekunneissa, kuinka kauan kettu voi juosta")]
        [SerializeField] private float maxRunDuration; //maksimi m‰‰r‰ sekunneissa, kuinka kauan kettu voi juosta
        [Header("aika sekunneissa, kuinka kauan menee ett‰ voi juosta taas....")]
        [SerializeField] private float maxRunCoolDown; //aika sekunneissa, kuinka kauan menee ett‰ voi juosta taas....


        private bool isRunning;//isRunning on true, kun painetaan shifti‰ pohjassa. 
        private float currentRunDuration, currentRunCoolDown;
        [HideInInspector] public float currentJumpDuration;
        public int jumpCount;//k‰ytet‰‰n hyppyyn...


        [Space(10)]
        [Header("t‰m‰ osa ketusta k‰‰ntyy, koska kettu ei k‰‰nn‰ koko kroppaa kerrallla...")]
        [SerializeField] private Transform IkRotationTransform;


        [Space (30)]
        [Header("post processsing homma lol")]
        [SerializeField] private Volume postProcessingVolume;
        private Vignette vignette;


        /// <summary>
        /// PORTAAT !!
        /// </summary>
        /// 
        [Space(20)]
        [Header("t‰t‰ paskaa ei k‰ytet‰ peliss‰")]
        [SerializeField] private FoxStairMovement foxStairMovement;



        [Space(20)]
        [Header("slider, josta n‰kee, kuinka kauan pelaaja ei voi liikkua....")]
        [SerializeField] Slider isSeenSlider;

        [Header("kuinka kauan odotetaan, kun kettu on n‰hty..., ja kuinka kauan on odotettu...")]
        [SerializeField] float waitAmount = 3, currentTime;


        [Space(20)]
        [Header("ketun ‰‰niefektit")]
        [SerializeField] private AudioSource walkAudio, landAudio;


        public MovementMode movementMode;

        public enum MovementMode
        {
            walkMode,           //kun kettu k‰velee
            flyMode,            //Kun kettu on ilmassa,
            squatMode,          //kun kettu on kyykyss‰
            hideMode,           //kun ollaan piilossa...
            idle,               //idle mode
            transitioning       //kun ollaan menossa uuteen sceneen....
        }

        private void Awake()
        {
            inputMaster = new InputMaster();

            isGrounded = true;

            jumpCount = 1;
            isGrounded = isItReallyGrounded();

            hunterFov.OnFoxSeen += FoxSeen;
        }

        #region enabledisable

        private void OnEnable()
        {
            inputMaster.Enable();

            inputMaster.Player.Move.performed += _  => FoxMove(_.ReadValue<Vector2>());
            inputMaster.Player.Move.canceled += _ => FoxMove(Vector2.zero);



            inputMaster.Player.Jump.performed += _ => FoxJump(true);
            inputMaster.Player.Jump.canceled += _ => FoxJump(false);

            inputMaster.Player.squashHide.performed += _ => FoxSquash(true); //aloitetaan kyykk‰‰minen
            inputMaster.Player.squashHide.canceled += _ => FoxSquash(false); //lopetetaan kyykk‰‰minen
        }

        private void OnDisable()
        {
            inputMaster.Disable();

            inputMaster.Player.Move.performed -= _ => FoxMove(_.ReadValue<Vector2>());
            inputMaster.Player.Move.canceled -= _ => FoxMove(Vector2.zero);



            inputMaster.Player.Jump.performed -= _ => FoxJump(true);
            inputMaster.Player.Jump.canceled -= _ => FoxJump(false);
            inputMaster.Player.squashHide.performed -= _ => FoxSquash(true); //aloitetaan kyykk‰‰minen
            inputMaster.Player.squashHide.canceled -= _ => FoxSquash(false); //lopetetaan kyykk‰‰minen
        }
        #endregion

        // Update is called once per frame
        void FixedUpdate()
        {
            if(isStopped)
                return;


            Debug.Log("jumpcount = " + jumpCount);
            if (movementAxis != Vector3.zero)        //kun liikutaan, tehd‰‰n liikkumisanimaatiot....
            {
                //foxStairMovement.CheckForChairs();//katsotaan portaiden varalta... (ehk‰ voisi laittaa if(isStairZone) hommaan, mutta pitt‰‰ testailla)

                float dotProduct = Vector3.Dot(movementAxis.normalized, transform.forward);

                Quaternion lookRot = Quaternion.LookRotation(movementAxis);

                Quaternion lookRotLocal = Quaternion.LookRotation(IkRotationTransform.InverseTransformDirection(movementAxis));

                float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

                if (angle + lookRotLocal.y >= 180)
                {
                    IkRotationTransform.localRotation = Quaternion.Euler(0, angle, 0);
                }
                else if (angle + lookRotLocal.y <= -180)
                {
                    IkRotationTransform.localRotation = Quaternion.Euler(0, angle, 0);
                }
                else
                {
                    IkRotationTransform.rotation = Quaternion.Lerp(IkRotationTransform.rotation, lookRot, fasterTurnSpeed * Time.deltaTime);
                }
                

                


                if (dotProduct < 0)
                {
                    //Vector3 tempMovementAxis = Vector3.Lerp(IkRotationTransform.forward, movementAxis, turnSpeed * Time.deltaTime);
                    transform.rotation = Quaternion.Lerp(transform.rotation, IkRotationTransform.rotation, fasterTurnSpeed * Time.deltaTime);

                }
                else
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, IkRotationTransform.rotation, turnSpeed * Time.deltaTime);
                }
                foxAnimator.SetBool("isMoving", true);

                if (jumpCount > 0 && !foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(jump) 
                    && !foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(land) 
                    && !foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(fly))
                    MovementAnim();

            }
            else if (movementAxis == Vector3.zero && jumpCount > 0)//muuten on vain idelAnimaatio
            {
                //IkRotationTransform.localRotation = Quaternion.Euler(0, 0, 0);
                if (!foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(idle) 
                    && !isSquashing 
                    && !foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(jump) 
                    && !foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(land)
                    && !foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(fly))
                {
                    foxAnimator.Play(idle);
                    
                }

                foxAnimator.SetBool("isMoving", false);

                movementMode = MovementMode.idle;

                if (walkAudio.isPlaying)    //k‰vely‰‰ni ei saa kuulua, kun ei liikuta...
                    walkAudio.Stop();

            }

            if(jumpCount < 1)
            {
                if (walkAudio.isPlaying)    //k‰vely‰‰ni ei saa kuulua, kun ollaan ilmassa...
                    walkAudio.Stop();
            }
            
            rb.velocity = RunSpeed(isRunning);
        }

        void Update() //joka frame juttuja tehd‰‰n...
        {
            if (isStopped)
            {
                if (walkAudio.isPlaying)    //k‰vely‰‰ni ei saa kuulua, kun ei liikuta...
                    walkAudio.Stop();

                return;
            }
                

            if (movementMode == MovementMode.transitioning)//ei tehd‰ mit‰‰n, kun ollaan menossa uuteen sceneen...
                return;

            foxAnimator.SetInteger("jumpCount", jumpCount);

            Debug.Log("isSquashing = " + isSquashing);

            if (isJumping)
            {
                ///Debug.Log("hypyn pit‰s toimia wtf" + currentJumpDuration);
                if (currentJumpDuration > 1)           //jos nappia on painettu pohjaan yli sekunnin...
                {
                    isJumping = false;
                    return;
                }


                if (currentJumpDuration < 0.5f)
                    currentJumpDuration = 0.5f;    //laitetaan minimiksi 0.3, sill‰ ei haluta, ett‰ aluksi ei hyp‰t‰ yht‰‰n ja sitten yht‰kki‰ kettu alkaa lent‰m‰‰n...

                ApplyJumpingForce();    //hyp‰t‰‰n
                currentJumpDuration += Time.deltaTime * 1;

                if (walkAudio.isPlaying)        //k‰vely‰‰ni ei saa kuulua, kun kettu on ilmassa...
                    walkAudio.Stop();
            }

            

        }

        #region foxJump
        private void ApplyJumpingForce()
        {
            if (isStopped || isHiding)
                return;

            Vector3 jumpVector = Vector3.up * jumpForce + Vector3.up * currentJumpDuration * 2;//hyppy vektori, joka on rigidbodyyn lis‰tt‰v‰n voiman vektori skaalattuna voimakkuudella.
                                                                                               //ja sen lis‰ksi skaalataan alasp‰in 0-1 sen perusteella, kuinka kauan on pidetty hyppy nappia pohjassa.

            Vector3 velocityVector = new Vector3(rb.velocity.x, jumpVector.y, rb.velocity.z);
            rb.velocity = velocityVector;
        }

        public bool startJump;
        private void FoxJump(bool enable)
        {
            
            if (movementMode == MovementMode.transitioning)//ei tehd‰ mit‰‰n, kun ollaan menossa uuteen sceneen...
                return;

            if (!enable)
            {
                Debug.Log("laitetaan jumoing pois...");
                isJumping = false;
                return;
            }
            if (!isGrounded)
                return;

            if (foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(jump) || foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(fly))
                return;

            if (startJump || isJumping || jumpCount == 0)
                return;

            foxAnimator.Play(jump); //hyppy animaatio...

            startJump = true;

            jumpCount = 0;
            currentJumpDuration = 0;
        }

        #endregion

        void MovementAnim() // t‰‰ll‰ valitaan, onko run vai walk animaatio vai kumpikaan...
        {
            if (movementMode == MovementMode.transitioning)//ei tehd‰ mit‰‰n, kun ollaan menossa uuteen sceneen...
                return;

            if (isHiding || !isGrounded || isSquashing || isStopped)
                return;


            jumpCount = 1;
            foxAnimator.SetInteger("jumpCount", jumpCount);

            if (!foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(walk))
                    foxAnimator.Play(walk);

            if(!walkAudio.isPlaying)        //jos k‰velyu‰‰nt‰ ei kuulu, se laitetaan p‰‰lle....
                walkAudio.Play();
        }


        Vector3 movementAxis;
        private void FoxMove(Vector2 axis)
        {
            if (isStopped)
                return;

            movementAxis = new Vector3(axis.x, 0, axis.y);

        }

        private Vector3 RunSpeed(bool running)
        {
            Vector3 roatatedVector = (Quaternion.Euler(body.rotation.x, body.rotation.y, body.rotation.z) * movementAxis);



            if (movementAxis != Vector3.zero)
                movementMode = MovementMode.walkMode; //laitetaan walkMode...

            return roatatedVector * speed + transform.up * rb.velocity.y; //kettu liikkuu...

        }




        /// <summary>
        /// Squash the fox so he can squash between small objects. If <paramref name="enabled"/> = true, squash, otherwise, rise up..s
        /// </summary>
        /// <param name="enabled"></param>
        private void FoxSquash(bool enabled)
        {
            if (movementMode == MovementMode.transitioning)//ei tehd‰ mit‰‰n, kun ollaan menossa uuteen sceneen...
                return;

            if (!isItReallyGrounded())
            {
                isGrounded = false;
                foxAnimator.Play(fly);
            }
            

            if (enabled)
            {
                foxAnimator.Play(squashStartStrin);

                isSquashing = true;

                if (isHiding)
                {


                    postProcessingVolume.profile.TryGet<Vignette>(out vignette);

                    if (vignette == null)
                        return;

                    vignette.intensity.value = 0.6f;

                }
            }
            else if (!enabled)
            {
                isSquashing = false;
                foxAnimator.Play(squashEndString);

                if (isHiding)
                {
                    if (waitToSquashBackUp)
                        return;

                    postProcessingVolume.profile.TryGet<Vignette>(out vignette);

                    if (vignette == null)
                        return;

                    vignette.intensity.value = 0.6f;

                }
            }
        }

        /// <summary>
        /// stop squashing is played when the player stops squashing from the animation...
        /// </summary>
        public void StopSquashing()
        {
            //Debug.Log("stopSquashing");
            
            isSquashing = false;
        }


        //freezataan kettu jne kun kettu n‰hd‰‰n....
        private void FoxSeen()
        {

            Debug.Log("isSeen juttu pit‰s menn‰ p‰‰lle....");
            currentTime = waitAmount;

            isSeenSlider.gameObject.SetActive(true);
            isSeen = true;
            


            StartCoroutine(FoxSeenCoroutine());

        }

        IEnumerator FoxSeenCoroutine()
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);

            yield return new WaitForSeconds(3);

            isSeen = false;
            isSeenSlider.gameObject.SetActive(false);

        }

        


        #region CheckGroundedStage

        /// <summary>
        /// this checks if the player actually fell from an object, or if the player jumped.... Returns either true when he fell, or false when he jumped....
        /// </summary>
        /// <returns></returns>
        private bool isItReallyGrounded()
        {
            RaycastHit hit;

            bool rayCastTest = Physics.Raycast(transform.position, -transform.up, out hit);

            if (rayCastTest)
                if (!hit.transform.CompareTag("wall"))
                {
                    float dist = Vector3.Distance(hit.point, transform.position);

                    if (dist <= 0.3f)
                        return true;
                }
                    

            return false;
        }



        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("wall"))
            {

                if (!landAudio.isPlaying)
                {
                    landAudio.Play();
                }
                
                isGrounded = true;
                jumpCount = 1;

                if (foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(jump) || foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(fly))
                {
                    foxAnimator.Play(land);
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!collision.gameObject.CompareTag("wall"))
            {
                /*
                if (Vector3.Dot(collision.contacts[0].normal, Vector3.up) < 1)
                {
                    isGrounded = true;
                    jumpCount = 1;
                }*/

                isGrounded = true;
                jumpCount = 1;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!collision.gameObject.CompareTag("wall"))
            {
                /*
                if (isItReallyGrounded() && jumpCount > 0) //kun jumpcount on yli 0. Jos se on 0 tai alle, on hyp‰tty eik‰ tiputtu
                    return;
                */
                if (!isItReallyGrounded() && !foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(jump))
                {
                    isGrounded = false;
                    foxAnimator.Play(fly);
                    FoxSquash(false);
                }
                if (!isItReallyGrounded())
                {
                    isGrounded = false;
                    jumpCount = 0;
                }

                
            }
        }



        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("UnderBed") || other.gameObject.CompareTag("hideZone"))
            {
                isHiding = true; 
                if (!isSquashing) //ei voi menn‰ piiloon, jos ei ole kyykyss‰..
                    return;

                if(other.gameObject.CompareTag("UnderBed"))
                    waitToSquashBackUp = true;


                postProcessingVolume.profile.TryGet<Vignette>(out vignette);

                if (vignette == null)
                    return;

                vignette.intensity.value = 0.6f;

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("UnderBed") || other.gameObject.CompareTag("hideZone"))
            {
                isHiding = false;

                if (waitToSquashBackUp)
                {
                    FoxSquash(false);
                    waitToSquashBackUp = false;
                }

                postProcessingVolume.profile.TryGet<Vignette>(out vignette);

                if (vignette == null)
                    return;

                vignette.intensity.value = 0.3f;
            }
        }

        #endregion
    }
}

