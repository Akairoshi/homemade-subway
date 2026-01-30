using UnityEngine;

public class ScoreBySpeed : MonoBehaviour, IUpdatable
{
    private RailsMover _mover;
    
    [Header("Добавление очков")]
    [SerializeField] private float baseInterval = 1f;
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField] private float minInterval = 0.1f;

    private float _timer;

    private void Awake()
    {
        _mover = Object.FindFirstObjectByType<RailsMover>();
    }
    public void OnUpdate(float deltaTime)
    {
        float speed = _mover.CurrentSpeed;

        float interval = baseInterval * (baseSpeed / speed);
        interval = Mathf.Max(interval, minInterval);

        _timer += deltaTime;

        if (_timer >= interval)
        {
            ScoreManager.Instance.AddScore(1);
            _timer -= interval;
        }
    }

    private void OnEnable()
    {
        if (UpdateManager.Instance != null)
            UpdateManager.Instance.Register(this);
    }

    private void OnDisable()
    {
        UpdateManager.Instance.Unregister(this);
    }

}
