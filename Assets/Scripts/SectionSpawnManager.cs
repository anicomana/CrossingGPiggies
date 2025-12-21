using UnityEngine;

public class SectionSpawnManager : MonoBehaviour
{
    //where to listen events from
    GroundManager groundManager;
    GameObject groundManagerObject;
    

    public GameObject[] sectionToSpawn;
    public int nInitialSections = 5;
    public float addSectionDistance = 2f;
    public float outBoundBottom = -7f;
    public float outBoundSide = 12f;

    private Vector3 outBound;
    private Vector3 firstSectionPos = new Vector3(0, 0, 6);
    private Vector3 lastSectionSpawnPos;


    //on awake finds gameobjects in scene
    void Awake()
    {
        groundManagerObject = GameObject.Find("_GroundManager");
    }

    void Start()
    {
        //if groundManagerObject is not null, then get component and subscribes to groundManager event
        if (groundManagerObject != null) {
            groundManager = groundManagerObject.GetComponent<GroundManager>();
            groundManager.OnMovedForward += InstantiateRandomSection;
        }

        lastSectionSpawnPos = firstSectionPos;
        InstantiateRandomSection();

        //for cycle to spawn next sections nCycles times in lastSectionSpawnPos
        for (int i = 0; i < nInitialSections; i++) {
            lastSectionSpawnPos.z += addSectionDistance;
            InstantiateRandomSection();
        }  
    }

    void Update()
    {

    }

    //this method is also called when event from groundManager is called, OnMovedForward
    void InstantiateRandomSection()
    {
        //to instantiate a random section from the list
        int randomSection = Random.Range(0, sectionToSpawn.Length);
        Instantiate(sectionToSpawn[randomSection], lastSectionSpawnPos, Quaternion.identity);
    }
}
