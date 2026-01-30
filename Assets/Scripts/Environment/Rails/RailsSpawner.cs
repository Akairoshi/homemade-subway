using System.Collections.Generic;
using UnityEngine;

public class RailsSpawner : MonoBehaviour
{
    [Header("Рельсы")]
    [SerializeField] private List<GameObject> _railPrefabs;
    [SerializeField] private int _initialRailsCount = 5;
    [SerializeField] private float _railLength = 10f;

    [Header("Препятствия")]
    [SerializeField] private List<GameObject> _obstaclePrefabs;
    [SerializeField, Range(0, 100)] private int _spawnChance = 50;

    private float[] _lanes = { -2f, 0f, 2f };
    private Queue<GameObject> _activeRails = new Queue<GameObject>();
    private float _nextSpawnZ;

    private void Awake()
    {
        _nextSpawnZ = 0f;
    }
    private void Start()
    {
        //создание стартовых рельс
        for (int i = 0; i < _initialRailsCount; i++)
        {
            SpawnRail();
        }
    }

    //Спавн рельс с обновлением препятствий
    public void SpawnRail()
    {
        GameObject prefab = _railPrefabs[Random.Range(0, _railPrefabs.Count)];

        GameObject rail = Instantiate(prefab, transform);

        rail.transform.localPosition = new Vector3(0f, 0f, _nextSpawnZ);

        //Создание и обновление препятсвий
        if (ScoreManager.Instance.score >= 1) {
        CreateObstaclePool(rail);
            UpdateObstaclesOnRail(rail);
        }
        //добавление префабов в очередь
        _activeRails.Enqueue(rail);
        _nextSpawnZ += _railLength;
    }

    //перемещение самой задней рельсы вперед с обновлением препятсвий
    public void RecycleRail()
    {
        Debug.Log("Рельса перемещена!");
        if (_activeRails.Count == 0) return;

        GameObject rail = _activeRails.Dequeue();

        rail.transform.localPosition = new Vector3(0f, 0f, _nextSpawnZ);
        
        if (ScoreManager.Instance.score >= 1)
            UpdateObstaclesOnRail(rail);

        _activeRails.Enqueue(rail);
        _nextSpawnZ += _railLength;
    }

    private void CreateObstaclePool(GameObject rail)
    {
        //Создание префабов препятский из списка префабов
        foreach (var prefab in _obstaclePrefabs)
        {
            GameObject obs = Instantiate(prefab, rail.transform);
            obs.SetActive(false);
            obs.tag = "Obstacle";
        }
    }

    //Обновление префабов препятсвий на рельсах
    private void UpdateObstaclesOnRail(GameObject rail)
    {
        foreach (Transform child in rail.transform)
        {
            if (child.CompareTag("Obstacle"))
                child.gameObject.SetActive(false);
        }

        if (Random.Range(0, 100) > _spawnChance) return;

        List<Transform> obstacles = new List<Transform>();
        foreach (Transform child in rail.transform)
        {
           if (child.CompareTag("Obstacle"))
               obstacles.Add(child);
        }

        if (obstacles.Count == 0) return;

        Transform selected = obstacles[Random.Range(0, obstacles.Count)];
        float laneX = _lanes[Random.Range(0, _lanes.Length)];

        selected.localPosition = new Vector3(laneX, 0.5f, 0f);
        selected.gameObject.SetActive(true);
    }
}
