using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public event System.Action OnEnemyCollision;
    public event System.Action<int> OnBonusCollision;
    public ParticleSystem waterParticle;
    public ParticleSystem smashParticle;
    public ParticleSystem bonusParticle;
    public int stepSizeSide = 1;
    public int stepSizeXAxis = 2;
    public float moveSpeed = 15f;
    public float posThreshold = 0.001f;
    public int playerOutBoundSide = 5;

    private Animator playerAnim;
    private Vector3 playerInitialPos;
    private GameObject player;
    private GameManager gameManager;
    private GameObject gameManagerObject;
    private Vector3 playerTargetPos;
    private bool playerMoving;
    private bool isGameOver;

    void Awake()
    {
        gameManagerObject = GameObject.Find("_GameManager");
    }

    void Start()
    {
        if (gameManagerObject != null) {
            gameManager = gameManagerObject.GetComponent<GameManager>();
            gameManager.OnGameOver += () => {isGameOver = true;};
        }

        playerAnim = GetComponent<Animator>();

        playerInitialPos = transform.position;
        playerTargetPos = transform.position;
        isGameOver = false;
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

        if (isGameOver == false) {
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
        }

        //to keep player inbound
        if (playerTargetPos.x < -playerOutBoundSide) {
            playerTargetPos = new Vector3(-playerOutBoundSide, playerTargetPos.y, playerTargetPos.z);
        } else if (playerTargetPos.x > playerOutBoundSide) {
            playerTargetPos = new Vector3(playerOutBoundSide, playerTargetPos.y, playerTargetPos.z);
        }
    }
 
    private void OnTriggerEnter(Collider other)
    {
        //check what's colliding
        switch (other.gameObject.tag) {
            case "Enemy":
                OnEnemyCollision?.Invoke();
                waterParticle.Stop();
                smashParticle.Play();
                //playerAnim.SetTrigger(); Death animation when dies
                break;

            case "Bonus":
                BonusController bonus = other.GetComponent<BonusController>();

                //invokes event and gives paramenter to whoever is listening (ScoreManager)
                if(bonus != null) {
                    int bonusPointsValue = bonus.ReturnPointsValue();
                    OnBonusCollision?.Invoke(bonusPointsValue);
                }

                bonusParticle.Play();
                Destroy(other.gameObject);
                break;
        }
    }
    
}
