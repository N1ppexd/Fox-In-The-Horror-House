using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoxMovement : MonoBehaviour
{
    public InputMaster inputMaster;


    [SerializeField] Rigidbody rb; //rigidbody ketulle...
    [SerializeField] private float speed, jumpForce, runSpeed; //speed on liikkuminen, jump force on hyppy...

    [SerializeField] private Animator foxAnimator;
    [SerializeField] private string squashStartStrin, squashEndString, walk, run, jump, idle;

    [SerializeField] private Transform body;


    private bool isUnderBed, waitToSquashBackUp; //isUnderbed on kun ollaan s�ngyn alla, waitTOSquashBackUp on kun ei paineta squash nappia kun ollaan s�ngyn alla

    private bool isGrounded;//kun kettu on maassa

    [SerializeField] private float turnSpeed; //nopeus, jolla kettu k��ntyy


    private bool isRunning;



    private int jumpCount;//k�ytet��n hyppyyn...

    private void Awake()
    {
        inputMaster = new InputMaster();

        inputMaster.Player.Move.performed += _ => FoxMove(_.ReadValue<Vector2>());
        inputMaster.Player.Move.canceled += _ => FoxMove(_.ReadValue<Vector2>());

        inputMaster.Player.Run.performed += _ => isRunning = true;
        inputMaster.Player.Run.canceled += _ => isRunning = false;



        inputMaster.Player.Jump.performed += _ => FoxJump();

        inputMaster.Player.squashHide.performed += _ => FoxSquash(true); //aloitetaan kyykk��minen
        inputMaster.Player.squashHide.canceled += _ => FoxSquash(false); //lopetetaan kyykk��minen


        isGrounded = true;
    }

    private void OnEnable()
    {
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(movementAxis != Vector3.zero)
        {
            float angle = Vector3.Dot(Vector3.right, movementAxis);
            angle = Mathf.Acos(angle);
            angle = Mathf.Rad2Deg * angle;



            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movementAxis), turnSpeed * Time.deltaTime);
            //Quaternion.Euler(0, angle, 0);

            if (isRunning && !isUnderBed && isGrounded)
                if (!foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(run) && !foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(squashStartStrin))
                    foxAnimator.Play(run);
            if(!isRunning && !isUnderBed && isGrounded)
                if (!foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(walk) && !foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(squashStartStrin))
                    foxAnimator.Play(walk);
        }
        else
        {
            if (!foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(idle) && !foxAnimator.GetCurrentAnimatorStateInfo(0).IsName(squashStartStrin))
                foxAnimator.Play(idle);
        }

        rb.velocity = RunSpeed(isRunning);
    }

    Vector3 movementAxis;
    private void FoxMove(Vector2 axis)
    {
        Vector3 toBeMovemedAxis = new Vector3(axis.x, 0, axis.y);
        movementAxis = Vector3.Lerp(movementAxis, toBeMovemedAxis, turnSpeed).normalized;

        Debug.Log("axis = " + movementAxis);
        
    }

    private Vector3 RunSpeed(bool running)
    {
        if (!running)
            return movementAxis * speed + Vector3.up * rb.velocity.y; //kettu liikkuu...
        if (isRunning)
            return movementAxis * runSpeed + Vector3.up * rb.velocity.y; //kettu liikkuu...

        return Vector3.zero;
    }

    private void FoxJump()
    {
        if (!isGrounded)
            return;

        jumpCount = 0;

        Vector3 jumpDirVector = Vector3.up * jumpForce;
        rb.AddForce(jumpDirVector);
    }

    private void FoxSquash(bool enabled)
    {
        if (!isGrounded) //jos on ilmassa tai on s�ngyn alla....
            return;

        if (isUnderBed)
        {
            if (enabled)
                waitToSquashBackUp = false;
            if (!enabled)
                waitToSquashBackUp = true;

            return;
        }

        if (enabled)
        {
            foxAnimator.Play(squashStartStrin);
        }
        else if(!enabled)
            foxAnimator.Play(squashEndString);
    }



    private bool isItReallyGrounded()
    {
        RaycastHit hit;

        bool rayCastTest = Physics.Raycast(transform.position, -transform.up, out hit);

        if (rayCastTest)
            if (hit.collider.transform.CompareTag("floor") || hit.collider.transform.CompareTag("bed"))
                if (hit.distance < 0.1f)
                    return true;

        return false;
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("bed"))
        {

            if (isItReallyGrounded() && jumpCount < 1)
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
            if (isItReallyGrounded() && jumpCount > 0) //kun jumpcount on yli 0. Jos se on 0 tai alle, on hyp�tty eik� tiputtu
                return;

            isGrounded = false;
            foxAnimator.Play(jump);
            FoxSquash(false);
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
}
