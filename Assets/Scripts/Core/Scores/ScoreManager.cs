using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance { get; private set; }
    public int score { get; private set; }

    private void Awake()
    {
        score = 0;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void AddScore(int scoreCount)
    {
        score += scoreCount;
        Debug.Log(score);
    }
    

}
