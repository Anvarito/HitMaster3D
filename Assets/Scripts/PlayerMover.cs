using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMover : MonoBehaviour
{
    public bool IsRun { get; private set; }

    private NavMeshAgent _meshAgent;
    private Animator _animator;

    private int _isRunID;
    private int _isShootID;

    public UnityEvent OnReachPlatform;
    public UnityEvent OnMove;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _meshAgent = GetComponent<NavMeshAgent>();

        _isRunID = Animator.StringToHash("IsRun");
        _isShootID = Animator.StringToHash("IsShoot");

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = true;
        }
    }

    public void SetNewWayPoint(Vector3 wayPoint)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, wayPoint, NavMesh.AllAreas, path);
        bool noWay = (path.status == NavMeshPathStatus.PathInvalid);

        if (noWay)
        {
            Debug.LogError("Not correctly way!");
            _meshAgent.SetDestination(transform.position);
            _animator.SetBool(_isRunID, false);
        }
        else
        {
            IsRun = true;
            _animator.SetBool(_isRunID, IsRun);
            _meshAgent.SetDestination(wayPoint);
        }

        OnMove?.Invoke();
    }

    public void RotateToShootDirect(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.normalized.x, 0, direction.normalized.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 1);

        _animator.SetTrigger(_isShootID);
    }



    void Update()
    {
        if (!_meshAgent.pathPending)
        {
            if (_meshAgent.remainingDistance <= _meshAgent.stoppingDistance)
            {
                {
                    IsRun = false;
                    _animator.SetBool(_isRunID, IsRun);
                    OnReachPlatform?.Invoke();
                }
            }
        }
    }
}
