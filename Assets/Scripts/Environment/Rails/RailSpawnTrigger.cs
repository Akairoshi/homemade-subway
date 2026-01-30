using UnityEngine;

public class RailSpawnTrigger : MonoBehaviour
{
    private RailsSpawner _railsSpawner;

    private void Start()
    {
        _railsSpawner = Object.FindFirstObjectByType<RailsSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _railsSpawner.RecycleRail();
            _railsSpawner.SpawnRail();
        }
        
    }
}