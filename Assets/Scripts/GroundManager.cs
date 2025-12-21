using UnityEngine;

public class GroundManager : MonoBehaviour
{
    //creating public events that other scripts are subscribed to
    public event System.Action OnMovedForward;
    public event System.Action OnMovedBackward;

    void Start()
    {
        
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


}
