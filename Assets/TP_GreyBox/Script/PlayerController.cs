using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public bool canControl = true;

    Rigidbody _rigidbody = null;
    protected bool IsActive { get; private set; }

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!canControl) return;

        Vector3 direction = Vector3.zero;
        direction += Input.GetAxisRaw("Horizontal") * Vector3.right;
        direction += Input.GetAxisRaw("Vertical") * Vector3.forward;
        direction.Normalize();
        _rigidbody.velocity = direction * speed + Vector3.up * _rigidbody.velocity.y;
    }
}