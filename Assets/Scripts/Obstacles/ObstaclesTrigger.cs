using UnityEngine;

public class ObstaclesAction : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        SceneController.Instance.RestartScene();
    }
}
