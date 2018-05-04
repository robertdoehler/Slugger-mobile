using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SluggerController : MonoBehaviour
{

    public float groundDistance;

    private SluggerMoveStates moveState;
    private SluggerGroundState groundState;
    private Vector3 groundRayDir;

    private Vector3 moveDir;
    private Rigidbody rigidbody;
    private Transform model;
    private Vector3 smoothMoveVel;
    private int groundLayer;

    private void Start()
    {
        GroundState = SluggerGroundState.air;
        rigidbody = GetComponent<Rigidbody>();
        model = transform.GetChild(0);
        groundLayer = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {

        //Get input
        if (Input.GetKeyDown(KeyCode.Space))
            MoveState = SluggerMoveStates.roll;
        else if (Input.GetKeyUp(KeyCode.Space))
            MoveState = SluggerMoveStates.crawl;

        //Get movement direction
        if (rigidbody.velocity.z < 0)
            moveDir = Vector3.forward;
        else
            moveDir = Vector3.back;

        //Flip model orientation based on movedir
        model.localScale = new Vector3(1, 0.7f, moveDir.z);

        //Stick to ground and rotate if sticky
        if (groundState == SluggerGroundState.sticky)
        {
            Ray groundRay = new Ray(transform.position, groundRayDir * -0.75f);
            RaycastHit groundhit;

            if (Physics.Raycast(groundRay, out groundhit, groundLayer))
            {
                Vector3 tempRayGroundDir = Vector3.Cross(transform.right, groundRayDir);

                Quaternion rawRot = Quaternion.LookRotation(tempRayGroundDir);
                if (Quaternion.Angle(rawRot, transform.rotation) < 25f)
                    transform.rotation = rawRot;

                Vector3 rawPos = groundhit.point + groundRayDir * groundDistance;
                if(Vector3.Distance(rawPos, transform.position) < 1f)
                    transform.position = rawPos;
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if(groundLayer == (groundLayer | (1 << collision.gameObject.layer)))
        {
            Vector3 contactPoint = collision.contacts[0].point;
            groundRayDir = collision.contacts[0].normal;

            if (MoveState == SluggerMoveStates.crawl && GroundState != SluggerGroundState.sticky)
            {
                Vector3 tempRayGroundDir = Vector3.Cross(transform.right, groundRayDir);
                transform.rotation = Quaternion.LookRotation(tempRayGroundDir);
                GroundState = SluggerGroundState.sticky;
            }
            else if (MoveState == SluggerMoveStates.roll && GroundState != SluggerGroundState.grounded)
                GroundState = SluggerGroundState.grounded;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (GroundState == SluggerGroundState.grounded)
            GroundState = SluggerGroundState.air;
    }

    public SluggerMoveStates MoveState
    {
        get
        {
            return moveState;
        }

        set
        {
            Debug.Log("MOVE_state changed to: " + value);
            moveState = value;
        }
    }

    public SluggerGroundState GroundState
    {
        get
        {
            return groundState;
        }

        set
        {
            Debug.Log("GROUND_state changed to: " + value);
            groundState = value;
        }
    }
}

public enum SluggerMoveStates
{
    crawl,
    roll
}

public enum SluggerGroundState
{
    grounded,
    sticky,
    air
}
