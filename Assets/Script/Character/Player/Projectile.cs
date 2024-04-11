using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    SphereCollider coll;
    public Rigidbody rb;

    public float speed;
    private void Awake()
    {
        coll = GetComponent<SphereCollider>();    
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Invoke("DespawnBullet",3f);
    }
    public void DespawnBullet()
    {
        SpawnManager.Instance.Despawn(gameObject);
    }
}