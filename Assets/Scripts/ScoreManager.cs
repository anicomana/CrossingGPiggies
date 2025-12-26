using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public event System.Action OnNewMaxReached;

    //where to listen events from
    GroundManager groundManager;
    GameObject groundManagerObject;

    private int currentSection = -2;
    private int maxSectionReached;
    private int scoreGainedPerSection = 1;
    void Awake()
    {
        groundManagerObject = GameObject.Find("_GroundManager");
    }

    void Start()
    {
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
        currentSection += scoreGainedPerSection;

        if (currentSection >= maxSectionReached) {
            maxSectionReached = currentSection;
            OnNewMaxReached.Invoke();

            if (maxSectionReached < 0) {
                maxSectionReached = 0;
            }

            Debug.Log("Current score is:" + maxSectionReached);

        }
    }

    //remove point when going backwards
    void RemoveCurrentSection()
    {
        currentSection -= scoreGainedPerSection;
    }

}
