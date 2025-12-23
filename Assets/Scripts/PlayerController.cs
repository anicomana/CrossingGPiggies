using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameObject player;

    //where to listen events from
    GroundManager groundManager;
    GameObject groundManagerObject;

    public float stepSizeSide = 1f;
    public float stepSizeXAxis = 2f;
    public float moveSpeed = 15f;
    public float posThreshold = 0.001f;

    public float playerOutBoundSide = 5f;
    
    private Vector3 playerTargetPos;
    private bool playerMoving;

    public event System.Action OnNewMaxReached;
    private int currentSection = -2;
    private int maxSectionReached;
    private int scoreGainedPerSection = 1;

    void Awake()
    {
        groundManagerObject = GameObject.Find("_GroundManager");
    }

    void Start()
    {
        playerTargetPos = transform.position;
        maxSectionReached = currentSection;

        
        if(groundManagerObject !=null) {
            groundManager = groundManagerObject.GetComponent<GroundManager>();
            groundManager.OnMovedForward += AddCurrentSection;
            groundManager.OnMovedBackward += RemoveCurrentSection;
        }
    }

    void Update()
    {

        //to move the player into the same position as playerTargetPos, and also snaps to the position
        if (playerMoving) {
            transform.position = Vector3.MoveTowards(transform.position, playerTargetPos, moveSpeed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, playerTargetPos) < posThreshold) {
                transform.position = playerTargetPos;
                playerMoving = false;
            }
            return;
        }

        Vector3 dir = Vector3.zero;

        //to decide in which direction the playerTargetPos will go
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            dir = Vector3.left;
        } else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            dir = Vector3.right;
        }
    
        //to move playerTargetPos when the direction is decided, then playerMoving is true
        if (dir != Vector3.zero) {
            playerTargetPos += dir * stepSizeSide;
            playerMoving = true;
        }

        //to keep player inbound
        if (playerTargetPos.x < -playerOutBoundSide) {
            playerTargetPos = new Vector3(-playerOutBoundSide, playerTargetPos.y, playerTargetPos.z);
        } else if (playerTargetPos.x > playerOutBoundSide) {
            playerTargetPos = new Vector3(playerOutBoundSide, playerTargetPos.y, playerTargetPos.z);
        }
    }

    //add points when going forward
    void AddCurrentSection()
    {
        currentSection += scoreGainedPerSection;

        if (currentSection > maxSectionReached) {
            maxSectionReached = currentSection;
            OnNewMaxReached?.Invoke();
            Debug.Log("Current score is:" + maxSectionReached);
        }
    }

    //remove point when going backwards
    void RemoveCurrentSection()
    {
        currentSection -= scoreGainedPerSection;
    }
}
