using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PoolObject))]
public class Bullet : MonoBehaviour
{
    public PoolObject PoolObject { get; private set; }

    private Vector3 _direction;
    private Rigidbody _rigidbody;

    private float _lifeDuration = 2;
    private float _lifeTimer = 0;

    public UnityEvent<Bullet> OnRemove;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        PoolObject = GetComponent<PoolObject>();
    }

    private void Update()
    {
        _lifeTimer += Time.deltaTime;
        if(_lifeTimer >= _lifeDuration)
        {
            _lifeTimer = 0;
            Deactivate();
        }
    }

    public void Activate(Vector3 dir, float speed)
    {
        _direction = dir.normalized;
        _rigidbody.AddForce(_direction * speed, ForceMode.Impulse);
    }

    private void Deactivate()
    {
        _rigidbody.velocity = Vector3.zero;
        _direction = Vector3.zero;

        OnRemove?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        RagDollElement ragdollElement;
        if (collision.transform.TryGetComponent(out ragdollElement))
        {
            ragdollElement.EnemyParent.BulletImpact();
            ragdollElement.Pushing(_direction);
        }

        Deactivate();
    }
}
