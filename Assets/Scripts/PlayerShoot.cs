using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private string _prefabName;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float _bulletSpeed = 0.15f;
    [SerializeField] private float _cooldown = 1;

    private PlayerMover _player;
    private float _cooldownTime = 0;

    void Start()
    {
        _player = GetComponent<PlayerMover>();
    }

    void Update()
    {
        if (_player.IsRun)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= _cooldownTime)
            {
                Shoot(Input.mousePosition);

                _cooldownTime = Time.time + _cooldown;
            }
        }
    }


    private void Shoot(Vector3 tapPoint)
    {
        Vector3 aimPoint = GetAimPoint(tapPoint);
        Vector3 direction = aimPoint - spawnPoint.position;

        _player.RotateToShootDirect(direction);

        Bullet bullet = _poolManager.GetObject(_prefabName, spawnPoint.position, Quaternion.identity).GetComponent<Bullet>();

        bullet.Activate(direction, _bulletSpeed);
        bullet.OnRemove.AddListener(ReturnBulletToPool);
    }

    private Vector3 GetAimPoint(Vector3 screenPoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        Vector3 aimPoint;
        if (Physics.Raycast(ray, out hit))
        {
            aimPoint = hit.point;
        }
        else
        {
            aimPoint = ray.origin + ray.direction * 1000;
        }

        return aimPoint;
    }

    private void ReturnBulletToPool(Bullet bullet)
    {
        bullet.OnRemove.RemoveListener(ReturnBulletToPool);
        _poolManager.ReturnObject(bullet.PoolObject);
    }
}
