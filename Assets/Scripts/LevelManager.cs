using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Platform> _platforms;
    [SerializeField] private PlayerMover _player;

    private int _numberPlatform = 0;
    private Platform _currentPlatform;

    private void Awake()
    {
        _player.OnReachPlatform.AddListener(PlayerReachedPoint);

        foreach(var p in _platforms)
        {
            p.OnCompletePlatform.AddListener(NextPlatform);
        }
    }

    private void Start()
    {
        _currentPlatform = _platforms[0];
    }

    private void PlayerReachedPoint()
    {
        if (_currentPlatform != null)
            _currentPlatform.PointHasBeenReach();
    }

    private void NextPlatform()
    {
        _numberPlatform += 1;
        if (_numberPlatform >= _platforms.Count)
        {
            SceneManager.LoadScene(0);
            return;
        }

        _currentPlatform = _platforms[_numberPlatform];
        _player.SetNewWayPoint(_currentPlatform.GetWayPoint().position);
    }
}
