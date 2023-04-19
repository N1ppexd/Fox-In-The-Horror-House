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

    private bool isGrounded;//kun kettu on maassa

    private void Awake()
    {
        inputMaster = new InputMaster();

        inputMaster.Player.Move.performed += _ => FoxMove(_.ReadValue<Vector2>());
        inputMaster.Player.Move.canceled += _ => FoxMove(_.ReadValue<Vector2>());

        inputMaster.Player.Jump.performed += _ => FoxJump();


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

    private void FoxSquash()
    {
        if (!isGrounded)
            return;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor"))
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor"))
            isGrounded = false;
    }
}
