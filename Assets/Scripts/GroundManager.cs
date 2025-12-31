using UnityEngine;
using System.Collections;

public class GroundManager : MonoBehaviour
{
    //creating public events that other scripts are subscribed to
    public event System.Action OnMovedForward;
    public event System.Action OnMovedBackward;

    //where to listen events from
    ScoreManager scoreManager;
    GameObject scoreManagerObject;
    GameManager gameManager;
    GameObject gameManagerObject;
    public GameObject[] sectionToSpawn;
    public GameObject startingBase;
    public int nInitialSections = 20;
    public float nextSectionDistance = 2f;
    public float outBoundBottom = -9f; //referenced in GroundMovement
    public float spawnDelay = 0.1f;
    public bool shouldSpawn = false;
    private Vector3 firstSectionPos;
    private Vector3 lastSectionSpawnPos;
    private bool isGameOver;
    private bool isGroundMoving;

    //on awake finds gameobjects in scene
    void Awake()
    {
        scoreManagerObject = GameObject.Find("_ScoreManager");
        gameManagerObject = GameObject.Find("_GameManager");
    }

    void Start()
    {
        //if groundManagerObject is not null, then get component and subscribes to groundManager event
        if (scoreManagerObject != null) {
            scoreManager = scoreManagerObject.GetComponent<ScoreManager>();
            scoreManager.OnNewMaxReached += () => {
                shouldSpawn = true; //when new max is reached a new section should spawn
            };
        }

        if (gameManagerObject != null) {
            gameManager = gameManagerObject.GetComponent<GameManager>();
            gameManager.OnGameOver += GroundGameOver;
        }
        firstSectionPos = new Vector3 (startingBase.transform.position.x, startingBase.transform.position.y, startingBase.transform.localScale.z);
        lastSectionSpawnPos = firstSectionPos;

        SpawnStartingSections();
    }

    void Update()
    {
        if (isGameOver == false) {

            //if player wants to move forward or backward, event is called
            //so every GameObjectthat has GroundMovement component can move
            //player can only move if previous movement is finished
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGroundMoving == false) {
                OnMovedForward?.Invoke();
                isGroundMoving = true;
            } else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && isGroundMoving == false) {
                OnMovedBackward?.Invoke();
                isGroundMoving = true;
            }
        }
    }

    //this method is also called when event from groundManager is called, OnMovedForward
    void InstantiateRandomSection()
    {
        //to instantiate a random section from the list
        int randomSection = Random.Range(0, sectionToSpawn.Length);
        Instantiate(sectionToSpawn[randomSection], lastSectionSpawnPos, Quaternion.identity);
        shouldSpawn = false;
    }

    void InstantiateStartingBase()
    {
        Instantiate(startingBase, startingBase.transform.position, Quaternion.identity);
    }
    
    //method called when when ground movement is done (from groundMovement)
    public void SpawnNewSectionIfNeeded() {

        isGroundMoving = false; //declares locally that ground is not moving anymore

        //checks if a new max section has been reached, if positive then instantiates new section
        if (shouldSpawn) {
            InstantiateRandomSection();
        }
    }
    //to be called when Restart event is invoked
    void SpawnStartingSections()
    {
        InstantiateStartingBase();
        InstantiateRandomSection();

        //for cycle to spawn next sections nCycles times in lastSectionSpawnPos
        for (int i = 0; i < nInitialSections; i++) {
            lastSectionSpawnPos.z += nextSectionDistance;
            InstantiateRandomSection();
        }  

    }

    //when receiving event it's game over
    void GroundGameOver()
    {
        isGameOver = true;
    }

}
