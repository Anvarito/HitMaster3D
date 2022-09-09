using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    public bool IsDead { get; private set; }

    private Rigidbody[] _rigidbodies;
    private Animator _animator;

    private Platform _platform;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _platform = GetComponentInParent<Platform>();

        _rigidbodies = GetComponentsInChildren<Rigidbody>();

    }

    void Start()
    {
        _animator.enabled = true;
        
        foreach (var r in _rigidbodies)
        {
            r.isKinematic = true;
            r.gameObject.AddComponent<RagDollElement>().EnemyParent = this;
        }
    }

    public void BulletImpact()
    {
        RagDollActivate();
    }

    private void RagDollActivate()
    {
        _animator.enabled = false;

        foreach (var r in _rigidbodies)
        {
            r.isKinematic = false;
        }

        IsDead = true;
        _platform.CompleteCheker();
    }
}
