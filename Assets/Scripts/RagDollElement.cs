using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollElement : MonoBehaviour
{
    public Enemy EnemyParent { get; set; }
    private Rigidbody _rigidbody;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Pushing(Vector3 direction)
    {
        _rigidbody.AddForce(direction * 170, ForceMode.Impulse);
    }
}
