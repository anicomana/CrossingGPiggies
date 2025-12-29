using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public event System.Action OnNewMaxReached;

    //where to listen events from
    GroundManager groundManager;
    GameObject groundManagerObject;
    GameObject startingSection;

    private float currentSection;
    private float maxSectionReached;
    private int scoreGainedPerSection = 1;
    void Awake()
    {
        groundManagerObject = GameObject.Find("_GroundManager");
        startingSection = GameObject.Find("StartingSection");
    }

    void Start()
    {
        currentSection = -startingSection.transform.position.z;
        maxSectionReached = currentSection;

        if(groundManagerObject !=null) {
            groundManager = groundManagerObject.GetComponent<GroundManager>();
            groundManager.OnMovedForward += AddCurrentSection;
            groundManager.OnMovedBackward += RemoveCurrentSection;
        }
    }

    void Update()
    {
        
    }

    //add points when going forward
    void AddCurrentSection()
    {
        Debug.Log("AddCurrentSection");
        currentSection += scoreGainedPerSection;

        if (currentSection > maxSectionReached) {
            maxSectionReached = currentSection;
            OnNewMaxReached.Invoke();
            Debug.Log("Current score is:" + maxSectionReached);
            Debug.Log("Current section in:" + currentSection);
        }
    }

    //remove point when going backwards
    void RemoveCurrentSection()
    {
        currentSection -= scoreGainedPerSection;
    }

}  
