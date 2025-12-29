using UnityEngine;
using System.Collections;
public class GameManager : MonoBehaviour
{
    PlayerController playerController;
    GameObject player;
    public event System.Action OnGameOver;
    //private bool isGameOver;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        if (player != null) {
            playerController = player.GetComponent<PlayerController>();
            //playerController.OnEnemyCollision += GameOver;
        }
    }

    void Update()
    {
        //if isGameOver true and press R then invoke Restart Event
    }

    void GameOver()
    {
        OnGameOver?.Invoke();
    }
}
