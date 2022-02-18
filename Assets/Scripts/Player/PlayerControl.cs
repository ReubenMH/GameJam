using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] Transform head;
    [SerializeField] Vector2 headLookBounds;

    [SerializeField] float walkSpeed;
    [SerializeField] float cameraLookSpeed;
    [SerializeField] float jumpForce;

    PlayerInventory inventory;
    PlayerFeet feet;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
        feet = GetComponentInChildren<PlayerFeet>();

        SetCursorBound(true);
    }

    private void Update()
    {
        ProcessLook();
        ProcessMovement();

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
        Vector2 mousePos = Input.mousePosition;

        Vector2 diff = oldMousePos - mousePos;
        float diffX = -1f * diff.x;
        transform.Rotate(transform.up, diffX * cameraLookSpeed * Time.deltaTime);

        float diffY = diff.y;
        headRot += diffY * cameraLookSpeed * Time.deltaTime;
        headRot = Mathf.Clamp(headRot, headLookBounds.x, headLookBounds.y);
        head.localEulerAngles = new Vector3(headRot, 0f, 0f);

        inventory.SetGunAngle(new Vector3(headRot, 0f, 0f));

        oldMousePos = mousePos;
    }

    void ProcessMovement()
    {
        float forward = Input.GetAxis("Vertical");
        float sideways = Input.GetAxis("Horizontal");

        Vector3 move = Vector3.zero;
        move += transform.forward * (forward * walkSpeed);
        move += transform.right * (sideways * walkSpeed);
        move.y = rigid.velocity.y;
        rigid.velocity = move;

        if (Input.GetButtonDown("Interact"))
        {
            inventory.AttemptPickup();
        }

        if (Input.GetMouseButton(0))
        {
            inventory.AttemptShoot();
        }

        if (Input.GetButtonDown("Jump"))
        {
            if(feet.IsGrounded)
                Jump();
        }

    }

    void Jump()
    {
        Vector3 velocity = rigid.velocity;
        velocity.y = jumpForce;
        rigid.velocity = velocity;
    }
}
