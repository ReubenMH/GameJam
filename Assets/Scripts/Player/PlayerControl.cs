using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] GrappleControl grapple;

    [SerializeField] Rigidbody rigid;
    [SerializeField] Transform head;
    [SerializeField] Vector2 headLookBounds;

    [SerializeField] float walkSpeed;
    [SerializeField] Vector2 cameraLookSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float grappleSpeed;
    [SerializeField] float grapplePullSpeed;

    PlayerInventory inventory;
    PlayerFeet feet;
    float forward = 0.0f;
    float sideways = 0.0f;

    // Grapple vars
    bool isPulled = false;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
        feet = GetComponentInChildren<PlayerFeet>();
        SetCursorBound(true);
    }

    private void Update()
    {
        forward = Input.GetAxis("Vertical");
        sideways = Input.GetAxis("Horizontal");

        ProcessLook();
        if (isPulled == false)
        {
            ProcessMovement();
        }

        if (Input.GetButtonDown("Cancel"))
            SetCursorBound(!isCursorBound);
    }

    bool isCursorBound = false;
    void SetCursorBound(bool b)
    {
        isCursorBound = b;
        Cursor.visible = !b;
        Cursor.lockState = b ? CursorLockMode.Confined : CursorLockMode.None;
    }

    Vector2 oldMousePos;
    float headRot = 0f;
    void ProcessLook()
    {
        float mouseX = Input.GetAxis("MouseX");
        transform.Rotate(transform.up, mouseX * cameraLookSpeed.x * Time.deltaTime);

        float mouseY = Input.GetAxis("MouseY");
        headRot -= mouseY * cameraLookSpeed.y * Time.deltaTime;
        headRot = Mathf.Clamp(headRot, headLookBounds.x, headLookBounds.y);
        head.localEulerAngles = new Vector3(headRot, 0f, 0f);

        inventory.SetGunAngle(new Vector3(headRot, 0f, 0f));
    }

    void ProcessMovement()
    {
        Vector3 move = Vector3.zero;
        move += transform.forward * (forward * walkSpeed);
        move += transform.right * (sideways * walkSpeed);
        move.y += rigid.velocity.y;
        rigid.velocity = move;

        if (Input.GetButtonDown("Interact"))
        {
            inventory.AttemptPickup();
        }

        if (Input.GetMouseButton(0))
        {
            inventory.AttemptShoot();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Grapple();
        }

        if (Input.GetButtonDown("Jump"))
        {
            if(feet.IsGrounded)
                Jump();
        }

    }

    void Grapple()
    {
        grapple.ShootGrapple(head.transform.forward, grappleSpeed);
    }

    public void UpdateGrapplePull(Vector3 dir)
    {
        rigid.velocity = grapplePullSpeed * dir;
        isPulled = true;
    }

    public void EndGrapplePull()
    {
        isPulled = false;
    }

    void Jump()
    {
        Vector3 velocity = rigid.velocity;
        velocity.y = jumpForce;
        rigid.velocity = velocity;
    }

}
