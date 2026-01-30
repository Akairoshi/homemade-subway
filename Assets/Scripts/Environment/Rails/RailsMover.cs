using UnityEngine;

public class RailsMover : MonoBehaviour, IUpdatable
{
    [Header("Скорость игры")]
    [SerializeField] private float _startSpeed = 10f;
    [SerializeField] private float _acceleration = 0.5f;
    [SerializeField] private float _maxSpeed = 50f;

    public float CurrentSpeed { get; private set; }

    private void Awake()
    {
        CurrentSpeed = _startSpeed;
    }

    public void OnUpdate(float deltaTime)
    {
        if (CurrentSpeed < _maxSpeed)
        {
            CurrentSpeed += _acceleration * deltaTime;
        }

        transform.position += Vector3.back * CurrentSpeed * deltaTime;
    }

    private void OnEnable()
    {
        UpdateManager.Instance?.Register(this);
    }

    private void OnDisable()
    {
        UpdateManager.Instance?.Unregister(this);
    }
}
