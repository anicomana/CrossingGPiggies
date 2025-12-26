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

    public GameObject[] sectionToSpawn;
    public int nInitialSections = 5;
    public float nextSectionDistance = 2f;
    public float outBoundBottom = -7f;
    public float outBoundSide = 12f;
    public float spawnDelay = 0.1f;

    private Vector3 outBound;
    private Vector3 firstSectionPos = new Vector3(0, 0, 6);
    private Vector3 lastSectionSpawnPos;

    //on awake finds gameobjects in scene
    void Awake()
    {
        scoreManagerObject = GameObject.Find("_ScoreManager");
    }

    void Start()
    {
        //if groundManagerObject is not null, then get component and subscribes to groundManager event
        if (scoreManagerObject != null) {
            scoreManager = scoreManagerObject.GetComponent<ScoreManager>();
            scoreManager.OnNewMaxReached += SpawnRandomSectionWithDelay;
        }

        lastSectionSpawnPos = firstSectionPos;

        InstantiateRandomSection();

        //for cycle to spawn next sections nCycles times in lastSectionSpawnPos
        for (int i = 0; i < nInitialSections; i++) {
            lastSectionSpawnPos.z += nextSectionDistance;
            InstantiateRandomSection();
        }  
    }

    void Update()
    {
        //if player wants to move forward or backward, event is called
        //so every GameObjectthat has GroundMovement component can move
         if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            OnMovedForward?.Invoke();
        } else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            OnMovedBackward?.Invoke();
        }
    }

    //this method is also called when event from groundManager is called, OnMovedForward
    void SpawnRandomSectionWithDelay()
    {
        //Adds delay on spawning new sections to prevent overlapping
        StartCoroutine(InstantiateWithDelay());
    }

    IEnumerator InstantiateWithDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        InstantiateRandomSection();
    }

    void InstantiateRandomSection()
    {
        //to instantiate a random section from the list
        int randomSection = Random.Range(0, sectionToSpawn.Length);
        Instantiate(sectionToSpawn[randomSection], lastSectionSpawnPos, Quaternion.identity);
        Debug.Log("New section spawned!");

    }
}
