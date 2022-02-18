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

    PlayerInventory inventory;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();

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
        //Cursor.lockState = b ? CursorLockMode.Locked : CursorLockMode.None;
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
        move += transform.forward * (forward * walkSpeed * Time.deltaTime);
        move += transform.right * (sideways * walkSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Interact"))
        {
            inventory.AttemptPickup();
        }

        if (Input.GetMouseButton(0))
        {
            inventory.AttemptShoot();
        }

        rigid.velocity = move;
    }
}
