using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private Scene _scene;
    public static SceneController Instance;
    public void Awake()
    {
        Instance = this;
    }
    public void RestartScene()
    {
        _scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(_scene.name);
        
    }
}
