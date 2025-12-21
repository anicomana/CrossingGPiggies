using UnityEngine;

//created a new type of variable: Direction that has, in this case, two values
enum Direction { Forward, Backward }

public class GroundMovement : MonoBehaviour
{
    //to read variables from
    PlayerController playerController;
    GameObject player;

    //where to listen events from
    GroundManager groundManager;
    GameObject groundManagerObject;

    private Vector3 groundTargetPos;
    private bool groundMoving;

    //on awake finds gameobjects in scene
    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        groundManagerObject = GameObject.Find("_GroundManager");
    }

    void Start()
    {
        groundTargetPos = transform.position;

        //check that player and groundManager are not null and the fetchin the components
        if (player != null) {
            playerController = player.GetComponent<PlayerController>();
        }

        if (groundManagerObject != null) {
            groundManager = groundManagerObject.GetComponent<GroundManager>();
            groundManager.OnMovedForward += MoveForward; //subscribes to event by groundManager
            groundManager.OnMovedBackward += MoveBackward; //subscribes to event by groundManager
        }
    }

    void Update()
    {
        //moves the section towards the target that has move
        if (groundMoving) {
            transform.position = Vector3.MoveTowards(transform.position, groundTargetPos, playerController.moveSpeed * Time.deltaTime);

            //if close enough, snaps to target's position
            if (Vector3.Distance(transform.position, groundTargetPos) < playerController.posThreshold) {
                transform.position = groundTargetPos;
                groundMoving = false;
            }
            return;
        }

        //if transform.position.z <= sectionSpawnManager.outBound
    }

    //when event is announced, calls Move method with input "type" Direction "value" Forward or Backward
    void MoveForward()
    {
        Move(Direction.Forward);
    }

    void MoveBackward()
    {
        Move(Direction.Backward);
    }

    //this method can only be called when has in input a value from "type" Direction, that we are calling direction
    void Move(Direction direction)
    {
        Vector3 dir = Vector3.zero;

        //check what "value", from "type" Direction, direction has
        switch (direction)
        {
            case Direction.Forward:
                dir = Vector3.back;
                break;
            case Direction.Backward:
                dir = Vector3.forward;
                break;
        }

        //once dir has been determined, we can calculate new groundTargetPos
        groundTargetPos += dir * playerController.stepSizeXAxis;
        groundMoving = true;
    }
}

