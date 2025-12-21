using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameObject player;

    public float stepSizeSide = 1f;
    public float stepSizeXAxis = 2f;
    public float moveSpeed = 15f;
    public float posThreshold = 0.001f;

    public float playerOutBoundSide = 5f;
    
    private Vector3 playerTargetPos;
    private bool playerMoving;
    
    void Start()
    {
        playerTargetPos = transform.position;
    }

    void Update()
    {
        if (playerMoving) {
            transform.position = Vector3.MoveTowards(transform.position, playerTargetPos, moveSpeed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, playerTargetPos) < posThreshold) {
                transform.position = playerTargetPos;
                playerMoving = false;
            }
            return;
        }

        Vector3 dir = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            dir = Vector3.left;
        } else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            dir = Vector3.right;
        }
        
        if (dir != Vector3.zero) {
            playerTargetPos += dir * stepSizeSide;
            playerMoving = true;
        }

        if (playerTargetPos.x < -playerOutBoundSide) {
            playerTargetPos = new Vector3(-playerOutBoundSide, playerTargetPos.y, playerTargetPos.z);
        } else if (playerTargetPos.x > playerOutBoundSide) {
            playerTargetPos = new Vector3(playerOutBoundSide, playerTargetPos.y, playerTargetPos.z);
        }
    }
}
