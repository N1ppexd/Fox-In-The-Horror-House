using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoxMovement : MonoBehaviour
{
    public InputMaster inputMaster;


    [SerializeField] Rigidbody rb; //rigidbody ketulle...
    [SerializeField] private float speed, jumpForce; //speed on liikkuminen, jump force on hyppy...

    [SerializeField] private Animator foxAnimator;
    [SerializeField] private string squashStartStrin, squashEndString;

    [SerializeField] private Transform body;


    private bool isUnderBed, waitToSquashBackUp; //isUnderbed on kun ollaan s‰ngyn alla, waitTOSquashBackUp on kun ei paineta squash nappia kun ollaan s‰ngyn alla

    private bool isGrounded;//kun kettu on maassa

    private void Awake()
    {
        inputMaster = new InputMaster();

        inputMaster.Player.Move.performed += _ => FoxMove(_.ReadValue<Vector2>());
        inputMaster.Player.Move.canceled += _ => FoxMove(_.ReadValue<Vector2>());

        inputMaster.Player.Jump.performed += _ => FoxJump();

        inputMaster.Player.squashHide.performed += _ => FoxSquash(true); //aloitetaan kyykk‰‰minen
        inputMaster.Player.squashHide.canceled += _ => FoxSquash(false); //lopetetaan kyykk‰‰minen


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
    void Update()
    {
        if(movementAxis != Vector3.zero)
        {
            
        }
    }

    Vector3 movementAxis;
    private void FoxMove(Vector2 axis)
    {
        movementAxis = new Vector3(axis.x, 0, axis.y);

        Debug.Log("axis = " + movementAxis);

        rb.velocity = movementAxis * speed  + Vector3.up * rb.velocity.y; //kettu liikkuu...
    }

    private void FoxJump()
    {
        if (!isGrounded)
            return;

        Vector3 jumpDirVector = Vector3.up * jumpForce;
        rb.AddForce(jumpDirVector);
    }

    private void FoxSquash(bool enabled)
    {
        if (!isGrounded) //jos on ilmassa tai on s‰ngyn alla....
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("bed"))
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("bed"))
        {
            isGrounded = false;
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
