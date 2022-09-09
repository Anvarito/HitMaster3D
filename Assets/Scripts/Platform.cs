using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform wayPoint;

    private List<Enemy> _enemies = new List<Enemy>();

    private bool _isReach = false;

    public UnityEvent OnCompletePlatform = new UnityEvent();

    private void Awake()
    {
        _enemies.AddRange(GetComponentsInChildren<Enemy>());
    }

    public Transform GetWayPoint()
    {
        return wayPoint;
    }

    public void PointHasBeenReach()
    {
        _isReach = true;
        CompleteCheker();
    }
    public void CompleteCheker()
    {
        if (IsComplete() && _isReach)
        {
            OnCompletePlatform?.Invoke();
        }
    }

    public bool IsComplete()
    {
        if (_enemies.Count > 0)
        {
            for (int i = 0; i < _enemies.Count; ++i)
            {
                if (!_enemies[i].IsDead)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
