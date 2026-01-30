using UnityEngine;
using System.Collections.Generic;

public class UpdateManager : MonoBehaviour
{
    public static UpdateManager Instance { get; private set; }
    private List<IUpdatable> _updatableList = new List<IUpdatable>();

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void Update()
    {
        for(int i = _updatableList.Count - 1; i >= 0; i--)
        {
            _updatableList[i]?.OnUpdate(Time.deltaTime);
        }
    }
    public void Register(IUpdatable updatable)
    {
        if (!_updatableList.Contains(updatable))
        {
            _updatableList.Add(updatable);
        }
    }
    public void Unregister(IUpdatable updatable)
    {
        if (_updatableList.Contains(updatable))
        {
            _updatableList.Remove(updatable);
        }
    }
}
