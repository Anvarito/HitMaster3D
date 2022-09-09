using UnityEngine;
//using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerMover _player;
    [SerializeField] private Vector3 _offset;
    private bool _isMoving = false;

    private void Awake()
    {
        _player.OnReachPlatform.AddListener(PlayerStop);
        _player.OnMove.AddListener(PlayerMove);
    }

    private void PlayerMove()
    {
        _isMoving = true;
    }

    private void PlayerStop()
    {
        _isMoving = false;
    }

    private void Update()
    {
        if(_isMoving)
        transform.position = _player.transform.position + _offset;
    }
}
