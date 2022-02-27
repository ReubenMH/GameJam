using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrappleState
{
    IDLE,
    EXTEND,
    RETRACT,
    GRIPPING
}

public class GrappleControl : MonoBehaviour
{
    [SerializeField] GrappleState state = GrappleState.IDLE;
    [SerializeField] float maxGrappleLength;
    [SerializeField] Rigidbody rigid;
    [SerializeField] PlayerControl player;

    LineRenderer grappleLine;

    Vector3 grappleDir;
    Vector3 oldPos;
    float grappleSpeed;
    float grapplePosDelta;

    // Start is called before the first frame update
    void Start()
    {
        grappleLine = GetComponentInChildren<LineRenderer>();
        GetComponent<MeshRenderer>().enabled = false;
        grappleLine.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        ProcessGrappleLine();
    }

    public void ShootGrapple(Vector3 dir, float speed)
    {
        if (state == GrappleState.IDLE)
        {
            grappleDir = dir;
            grappleSpeed = speed;
            SetState(GrappleState.EXTEND);
        }
    }

    void SetState(GrappleState newState)
    {
        if (state != newState)
        {
            state = newState;
            StartState();
        }
    }

    void StartState()
    {
        switch (state)
        {
            case GrappleState.IDLE:
                GetComponent<MeshRenderer>().enabled = false;
                grappleLine.enabled = false;
                break;
            case GrappleState.EXTEND:
                GetComponent<MeshRenderer>().enabled = true;
                grappleLine.enabled = true;
                transform.position = player.transform.position;
                oldPos = transform.position;
                grapplePosDelta = 0.0f;
                break;
        }
    }

    void UpdateState()
    {
        switch (state)
        {
            case GrappleState.IDLE:
                break;
            case GrappleState.EXTEND:
                ProcessExtend();
                break;
            case GrappleState.GRIPPING:
                ProcessGripping();
                break;
            case GrappleState.RETRACT:
                ProcessRetract();
                break;
        }
    }

    void ProcessExtend()
    {
        Vector3 move = grappleDir * grappleSpeed;
        rigid.velocity = move;
        grapplePosDelta += (oldPos - transform.position).magnitude;
        oldPos = transform.position;

        if (grapplePosDelta > maxGrappleLength)
        {
            SetState(GrappleState.RETRACT);
        }
    }

    void ProcessGripping()
    {
        Vector3 vecToPlayer = transform.position - player.transform.position;

        rigid.velocity = new Vector3();
        player.UpdateGrapplePull(vecToPlayer.normalized);

        if (vecToPlayer.magnitude < 1.0f)
        {
            SetState(GrappleState.IDLE);
            player.EndGrapplePull();
        }
    }

    void ProcessRetract()
    {
        Vector3 vecToPlayer = player.transform.position - transform.position;
        Vector3 move = vecToPlayer.normalized * grappleSpeed;
        rigid.velocity = move;

        if (vecToPlayer.magnitude < 1.0f)
        {
            SetState(GrappleState.IDLE);
        }
    }

    void ProcessGrappleLine()
    {
        grappleLine.SetPosition(0, player.transform.position);
        grappleLine.SetPosition(1, transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tree")
        {
            print("tree");
            if (state == GrappleState.EXTEND)
            {
                transform.position = other.ClosestPoint(player.transform.position);
                SetState(GrappleState.GRIPPING);
            }
        }
    }
}
