using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class SluggerController : MonoBehaviour
{

    #region vars
    public bool debugMode;
    public float slideFriction;
    public float gravityScale;

    private BehaveState behaveState;
    private GroundState groundState;
    private CircleCollider2D physicalCollider;
    private CircleCollider2D awarenessCollider;
    private new Rigidbody2D rigidbody;
    #endregion

    #region monobehaviours
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        physicalCollider = GetComponents<CircleCollider2D>()[0];
        awarenessCollider = GetComponents<CircleCollider2D>()[1];

        GroundState = GroundState.air;
        BehaveState = BehaveState.slide;
    }

    void Update()
    {
        //Switch between crawl and roll
        if (Input.GetKeyDown(KeyCode.Space))
            BehaveState = BehaveState.roll;
        else if (Input.GetKeyUp(KeyCode.Space))
            BehaveState = BehaveState.slide;
    }

    private void FixedUpdate()
    {
        //When touching no Ground, go air mode
        if (!awarenessCollider.IsTouchingLayers(GameStatics.layers["Ground"]) && GroundState != GroundState.air)
            GroundState = GroundState.air;

        //Stick to Ground if in Sticky mode
        if (GroundState == GroundState.sticky)
            BeSticky();

        if (GroundState == GroundState.air || GroundState == GroundState.grounded)
            RotateTowardsDirection(rigidbody.velocity);
    }

    #endregion

    #region sluggerBehaviour
    private void BeSticky()
    {
        rigidbody.velocity -= rigidbody.velocity.normalized * slideFriction;

        //Calculate radius for distance to be sticky
        float awarenessRadius = awarenessCollider.radius * transform.localScale.x;

        //Cast ground ray and check if hit ground
        Ray2D groundRay = new Ray2D(transform.position, -transform.up);
        RaycastHit2D groundRayHit = Physics2D.Raycast(groundRay.origin, groundRay.direction, awarenessRadius, GameStatics.layers["Ground"]);

        //If ground ray hit ground, stick Slugger to ground point and rotate
        if (groundRayHit.collider != null)
            stickToGround(groundRayHit.point, groundRayHit.normal);

        if (debugMode)
        {
            Debug.DrawRay(groundRay.origin, (groundRay.direction) * awarenessRadius, Color.red, 0.5f);
        }
    }

    private void stickToGround(Vector2 point, Vector2 normal)
    {
        //Get the Vector parallel to the ground hit
        Vector2 groundParallel = Vector3.Cross(Vector3.forward, normal);

        rigidbody.angularVelocity = 0;

        //ROTATE PARALLEL TO GROUND
        RotateTowardsDirection(groundParallel);

        //STICK TO GROUND POINT POSITION
        float physicalRadius = physicalCollider.radius * transform.localScale.x;
        transform.position = point + (normal * physicalRadius);

        //SET VELOCITY PARALLEL TO GROUND DIRECTION
        rigidbody.velocity = groundParallel.normalized * (rigidbody.velocity.magnitude * getMoveDir());

        if (debugMode)
            Debug.DrawRay(point, groundParallel * 10, Color.blue, 2);
    }

    private void RotateTowardsDirection(Vector3 lookDir)
    {
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 180;

        Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //if (Quaternion.Angle(transform.rotation, newRotation) >= 180)
        //{

        //}

        transform.localRotation = newRotation;

        //transform.localRotation = Quaternion.Slerp(transform.localRotation, newRotation, 0.2f);

    }

    private int getMoveDir()
    {
        Vector2 relativeVelocity = transform.InverseTransformDirection(rigidbody.velocity);
        //Get movement direction
        if (relativeVelocity.x < 0)
            return 1;
        else
            return -1;
    }
    #endregion

    #region collissionBehaviours
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (GroundState == GroundState.air)
            GroundState = GroundState.grounded;

        //Trigger Sticky Function only if grounded/Sliding and touching a ground object
        if (GameStatics.IsInLayerMask(collider.gameObject.layer, GameStatics.layers["Ground"]) && groundState == GroundState.grounded && behaveState == BehaveState.slide)
        {
            //Calculate radius for distance to be sticky
            float awarenessRadius = awarenessCollider.radius * transform.localScale.x;

            //Get collisionDist between both colliders
            Vector3 closestHitPoint = GameStatics.ClosestPointBetweenColliders(physicalCollider, collider);
            Vector3 collHitDirection = (closestHitPoint - transform.position).normalized;

            //Cast ground ray and check if hit ground
            Ray2D groundRay = new Ray2D(closestHitPoint, collHitDirection);
            RaycastHit2D groundRayHit = Physics2D.Raycast(groundRay.origin, groundRay.direction, awarenessRadius, GameStatics.layers["Ground"]);

            //If ground ray hit ground, stick Slugger to ground point and rotate
            if (groundRayHit.collider != null)
                stickToGround(groundRayHit.point, groundRayHit.normal);

            GroundState = GroundState.sticky;

            if (debugMode)
                Debug.DrawRay(closestHitPoint, collHitDirection * awarenessRadius * 2, Color.yellow, 3);

        }
        else if (behaveState == BehaveState.roll)
        {
            GroundState = GroundState.grounded;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        //Switch to groundMode air if exiting ground
        if (LayerMask.LayerToName(collider.gameObject.layer) == "Ground")
        {
            if (GroundState == GroundState.grounded)
                GroundState = GroundState.air;
        }
    }
    #endregion

    #region getterSetters
    public BehaveState BehaveState
    {
        get
        {
            return behaveState;
        }

        set
        {
            if (behaveState != value)
            {
                Debug.Log("MOVE_state changed to: " + value + " <----- !!!!!!!");
                behaveState = value;
            }


        }
    }

    public GroundState GroundState
    {
        get { return groundState; }

        set
        {
            if (groundState != value)
            {
                groundState = value;
                Debug.Log("GROUND_state changed to: " + value);
                if (value != GroundState.sticky)
                {
                    rigidbody.gravityScale = gravityScale;
                } else
                {
                    rigidbody.gravityScale = 0;
                }
            }
        }
    }
    #endregion
}