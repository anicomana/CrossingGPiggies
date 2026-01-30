using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public event System.Action OnNewMaxReached;
    public event System.Action MovedBackTooMuch;
    public event System.Action OnNewBonusReached;

    //where to listen events from
    GroundManager groundManager;
    GameObject groundManagerObject;
    GameManager gameManager;
    GameObject gameManagerObject;
    GameObject startingBase;
    PlayerController playerController;
    GameObject playerControllerObject;

    private float currentSection;
    private float maxSectionReached;
    private int minSectionsUntilBonus = 5;
    private int maxSectionsUntilBonus = 10;
    private int sectionsUntilNextBonus;
    private float bonusPoints;
    void Awake()
    {
        groundManagerObject = GameObject.Find("_GroundManager");
        gameManagerObject = GameObject.Find("_GameManager");
        startingBase = GameObject.FindGameObjectWithTag("StartingBase");
        playerControllerObject = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        ResetSCore();
        ResetSectionsUntilBonus();

        if (gameManagerObject !=null) {
            gameManager = gameManagerObject.GetComponent<GameManager>();
            gameManager.OnGameOver += CalculateFinalScore;
        }

        if(groundManagerObject !=null) {
            groundManager = groundManagerObject.GetComponent<GroundManager>();
            groundManager.OnMovedForward += AddCurrentSection;
            groundManager.OnMovedBackward += RemoveCurrentSection;
        }

        if (playerControllerObject != null) {
            playerController = playerControllerObject.GetComponent<PlayerController>();
            playerController.OnBonusCollision += AddBonusPoints;
        }
    }

    void Update()
    {
        float diff = maxSectionReached - currentSection;
        if (diff >= gameManager.maxBackwardsSteps) {
            MovedBackTooMuch?.Invoke();
        }
    }

    //add points when going forward
    void AddCurrentSection()
    {
        currentSection++;

        if (currentSection > maxSectionReached) {
            maxSectionReached = currentSection;
            OnNewMaxReached?.Invoke();
            Debug.Log("Current score is: " + maxSectionReached);

            sectionsUntilNextBonus--;
            if(sectionsUntilNextBonus <= 0) {
                ResetSectionsUntilBonus();
                OnNewBonusReached?.Invoke();
                Debug.Log("Next bonus reached");
            }
        }
    }

    //remove point when going backwards
    void RemoveCurrentSection()
    {
        currentSection--;
    }

    void CalculateFinalScore()
    {
        float finalScore = maxSectionReached + bonusPoints;
        Debug.Log("Streets crossed: " + maxSectionReached + " || Bonus Points: " + bonusPoints + " || Final Score " + finalScore);
    }
    //to be called when Restart event is Invoked
    void ResetSCore()
    {
        currentSection = -startingBase.transform.position.z;
        maxSectionReached = currentSection;
    }

    void ResetSectionsUntilBonus()
    {
        sectionsUntilNextBonus = Random.Range(minSectionsUntilBonus, maxSectionsUntilBonus + 1);
    }

    void AddBonusPoints(int point)
    {
         bonusPoints += point;
         Debug.Log("Bonus points: " + bonusPoints);
    }
}  
