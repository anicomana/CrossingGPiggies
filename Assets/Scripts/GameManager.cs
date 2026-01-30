using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameManager : MonoBehaviour
{
    PlayerController playerController;
    GameObject player;
    ScoreManager scoreManager;
    GameObject scoreManagerObject;
    public event System.Action OnGameOver;
    public int maxBackwardsSteps = 3;
    private bool isGameOver;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scoreManagerObject = GameObject.Find("_ScoreManager");
    }
    void Start()
    {
        if (player != null) {
            playerController = player.GetComponent<PlayerController>();
            playerController.OnEnemyCollision += GameOver;
        }

        if (player != null) {
            scoreManager = scoreManagerObject.GetComponent<ScoreManager>();
            scoreManager.MovedBackTooMuch += GameOver;
        }

        isGameOver = false;
    }
    void Update()
    {
        //if isGameOver true and press R then invoke Restart Event
        if (Input.GetKeyDown(KeyCode.R)) {
            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
        }
    }

    void GameOver()
    {
        if (isGameOver == false) {
            isGameOver = true;
            OnGameOver?.Invoke();
            Debug.Log("GAMEOVER");
        }
    }
}
