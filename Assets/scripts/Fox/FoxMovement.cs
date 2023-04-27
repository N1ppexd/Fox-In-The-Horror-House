using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace HorrorFox.Fox
{
    public class FoxMovement : MonoBehaviour
    {
        public InputMaster inputMaster;


        [SerializeField] Rigidbody rb; //rigidbody ketulle...
        [SerializeField] private float speed, jumpForce, runSpeed; //speed on liikkuminen, jump force on hyppy...

        [SerializeField] private Animator foxAnimator;
        [SerializeField] private string squashStartStrin, squashEndString, walk, run, jump, idle;

        [SerializeField] private Transform body;


        private bool isUnderBed, waitToSquashBackUp; //isUnderbed on kun ollaan s‰ngyn alla, waitTOSquashBackUp on kun ei paineta squash nappia kun ollaan s‰ngyn alla

        private bool isGrounded;//kun kettu on maassa

        private bool isSquashing;

        [Space]
        [SerializeField] private float turnSpeed; //nopeus, jolla kettu k‰‰ntyy

        [SerializeField] private float maxRunDuration; //maksimi m‰‰r‰ sekunneissa, kuinka kauan kettu voi juosta
        [SerializeField] private float maxRunCoolDown; //aika sekunneissa, kuinka kauan menee ett‰ voi juosta taas....


        private bool isRunning, isJumping;//isRunning on true, kun painetaan shifti‰ pohjassa. IsRunning on true, kun painetaan hyppy nappia pohjassa.
        [SerializeField] private float currentRunDuration, currentRunCoolDown, currentJumpDuration;
        private int jumpCount;//k‰ytet‰‰n hyppyyn...


        [SerializeField] private Transform IkRotationTransform;

        private void Awake()
        {
            inputMaster = new InputMaster();

            isGrounded = true;

            jumpCount = 1;
            isGrounded = isItReallyGrounded();
        }

        #region enabledisable

        private void OnEnable()
        {
            inputMaster.Enable();

            inputMaster.Player.Move.performed += FoxMove;
            inputMaster.Player.Move.canceled += FoxMove;

            inputMaster.Player.Run.performed += _ => isRunning = setRunningMode(true);
            inputMaster.Player.Run.canceled += _ => isRunning = setRunningMode(false);



            inputMaster.Player.Jump.performed += _ => FoxJump(true);
            inputMaster.Player.Jump.canceled += _ => FoxJump(false);

            inputMaster.Player.squashHide.performed += _ => FoxSquash(true); //aloitetaan kyykk‰‰minen
            inputMaster.Player.squashHide.canceled += _ => FoxSquash(false); //lopetetaan kyykk‰‰minen
        }

        private void OnDisable()
        {
            inputMaster.Disable();

            inputMaster.Player.Move.performed -= FoxMove;
            inputMaster.Player.Move.canceled -= FoxMove;

            inputMaster.Player.Run.performed -= _ => isRunning = setRunningMode(true);
            inputMaster.Player.Run.canceled -= _ => isRunning = setRunningMode(false);



            inputMaster.Player.Jump.performed -= _ => FoxJump(true);
            inputMaster.Player.Jump.canceled -= _ => FoxJump(false);
            inputMaster.Player.squashHide.performed -= _ => FoxSquash(true); //aloitetaan kyykk‰‰minen
            inputMaster.Player.squashHide.canceled -= _ => FoxSquash(false); //lopetetaan kyykk‰‰minen
        }
        #endregion

        // Update is called once per frame
        void FixedUpdate()
        {
            Debug.Log("jumpcount = " + jumpCount);
            if (movementAxis != Vector3.zero)        //kun liikutaan, tehd‰‰n liikkumisanimaatiot....
            {
                float angle = Vector3.Dot(Vector3.right, movementAxis);
                angle = Mathf.Acos(angle);
                angle = Mathf.Rad2Deg * angle;

                IkRotationTransform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movementAxis), 30 * Time.deltaTime);

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movementAxis), turnSpeed * Time.deltaTime);
                //Quaternion.Euler(0, angle, 0);
                MovementAnim();

            }
            else//muuten on vain idelAnimaatio
            {
                //IkRotationTransform.rotation = Quaternion.Euler(0, 0, 0);
                if (!foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(idle) && !isSquashing)
                    foxAnimator.Play(idle);
            }

            rb.velocity = RunSpeed(isRunning);
        }

        void Update() //joka frame juttuja tehd‰‰n...
        {
            Debug.Log("isSquashing = " + isSquashing);

            if (jumpCount < 1 || isSquashing)
                isRunning = false;
            if (currentRunCoolDown > 0f)//t‰m‰n avulla laitetaan niin, ettei voi juosta liikaa
            {
                currentRunCoolDown -= Time.deltaTime;
            }

            else if (currentRunCoolDown < 0)
                currentRunCoolDown = 0;

            if (currentRunDuration > 0f)
                currentRunDuration -= Time.deltaTime;


            else if (currentRunDuration <= 0)
            {
                currentRunDuration = 0;
                //currentRunCoolDown = maxRunCoolDown;
                isRunning = false;
                //setRunningMode(false);
            }

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
            }
        }

        #region foxJump
        private void ApplyJumpingForce()
        {

            Vector3 jumpVector = Vector3.up * jumpForce + Vector3.up * currentJumpDuration * 2;//hyppy vektori, joka on rigidbodyyn lis‰tt‰v‰n voiman vektori skaalattuna voimakkuudella.
                                                                                               //ja sen lis‰ksi skaalataan alasp‰in 0-1 sen perusteella, kuinka kauan on pidetty hyppy nappia pohjassa.

            Vector3 velocityVector = new Vector3(rb.velocity.x, jumpVector.y, rb.velocity.z);
            rb.velocity = velocityVector;
        }


        private void FoxJump(bool enable)
        {
            if (!enable)
            {
                Debug.Log("laitetaan jumoing pois...");
                isJumping = false;
                return;
            }
            if (!isGrounded)
                return;

            Debug.Log("isHoldingJumpButton");


            isJumping = true;           //laitetaan isJUmping trueksi, koska hyp‰t‰‰n..

            currentJumpDuration = 0;
            jumpCount = 0;
            return;



            //Vector3 jumpDirVector = Vector3.up * jumpForce;
            //rb.AddForce(jumpDirVector);
        }

        #endregion

        void MovementAnim() // t‰‰ll‰ valitaan, onko run vai walk animaatio vai kumpikaan...
        {
            if (isUnderBed || !isGrounded || isSquashing)
                return;

            jumpCount = 1;

            if (isRunning)
                if (!foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(run))
                    foxAnimator.Play(run);
            if (!isRunning)
                if (!foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(walk))
                    foxAnimator.Play(walk);
        }


        Vector3 movementAxis;
        private void FoxMove(InputAction.CallbackContext obj)
        {
            Vector2 axis = obj.ReadValue<Vector2>();

            Vector3 toBeMovemedAxis = new Vector3(axis.x, 0, axis.y);
            movementAxis = Vector3.Lerp(movementAxis, toBeMovemedAxis, turnSpeed).normalized;

            Debug.Log("axis = " + movementAxis);

        }

        /// <summary>
        /// Used to run, but also to stop running and to apply the 
        /// </summary>
        /// <param name="startRunning"></param>
        /// <returns></returns>
        private bool setRunningMode(bool startRunning)
        {
            if (startRunning && currentRunCoolDown <= 0) //jos aloitetaan juokseminen, ja cooldown on mennyt loppuun
            {
                currentRunDuration = maxRunDuration;

                Debug.Log("run pit‰s toimia");

                return true;
            }
            if (!startRunning && currentRunDuration > 0)
            {
                currentRunCoolDown = maxRunCoolDown - currentRunDuration;
                return false;
            }
            return startRunning;
        }

        private Vector3 RunSpeed(bool running)
        {
            if (!running)
                return movementAxis * speed + Vector3.up * rb.velocity.y; //kettu liikkuu...
            if (running)
                return movementAxis * runSpeed + Vector3.up * rb.velocity.y; //kettu liikkuu...

            return Vector3.zero;
        }




        /// <summary>
        /// Squash the fox so he can squash between small objects. If <paramref name="enabled"/> = true, squash, otherwise, rise up..s
        /// </summary>
        /// <param name="enabled"></param>
        private void FoxSquash(bool enabled)
        {
            if (!isItReallyGrounded())
            {
                isGrounded = false;
                foxAnimator.Play(jump);
            }
                
            if (!isGrounded) //jos on ilmassa tai on s‰ngyn alla....
                return;

            if (isUnderBed)
            {
                isSquashing = true;

                if (enabled)
                    waitToSquashBackUp = false;
                if (!enabled)
                    waitToSquashBackUp = true;

                return;
            }

            if (enabled)
            {
                foxAnimator.Play(squashStartStrin);

                isSquashing = true;
            }
            else if (!enabled)
                foxAnimator.Play(squashEndString);

            Debug.Log("fox squash enabled = " + enabled);
        }

        /// <summary>
        /// stop squashing is played when the player stops squashing from the animation...
        /// </summary>
        public void StopSquashing()
        {
            Debug.Log("stopSquashing");
            
            isSquashing = false;
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
                if (hit.collider.transform.CompareTag("floor") || hit.collider.transform.CompareTag("bed"))
                {
                    float dist = Vector3.Distance(hit.point, transform.position);

                    Debug.Log("distance to floor = " + dist);

                    if (dist <= 0.3f)
                        return true;
                }
                    

            return false;
        }



        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("bed"))
            {

                if (isItReallyGrounded())
                {

                    isGrounded = true;
                    jumpCount = 1;
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("bed"))
                isGrounded = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("bed"))
            {
                /*
                if (isItReallyGrounded() && jumpCount > 0) //kun jumpcount on yli 0. Jos se on 0 tai alle, on hyp‰tty eik‰ tiputtu
                    return;
                */
                if (!isItReallyGrounded())
                {
                    isGrounded = false;
                    foxAnimator.Play(jump);
                    FoxSquash(false);
                }

                
            }
        }



        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("UnderBed"))
            {
                isUnderBed = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("UnderBed"))
            {
                isUnderBed = false;

                if (waitToSquashBackUp)
                {
                    FoxSquash(false);
                    waitToSquashBackUp = false;
                }
            }
        }

        #endregion
    }
}

