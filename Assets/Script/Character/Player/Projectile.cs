using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor.PackageManager;
using UnityEditor.U2D;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    SphereCollider coll;

    public GameObject hitEffectPrefab;
    public float speed;
    private void Awake()
    {
        coll = GetComponent<SphereCollider>();    
    }

    private void Update()
    {
        Vector3 worldDirection = transform.TransformDirection(Vector3.forward);
        transform.position += worldDirection * speed * Time.deltaTime;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitColl;
        if (Physics.Raycast(ray, out hitColl, 0.5f))
        {
            if (hitColl.collider.transform.GetComponent<Monster>())
            {
                hitColl.transform.GetComponent<Monster>().GetDamage(GameManager.Instance.player.damage);
            }
            GameObject clone = LeanPool.Spawn(hitEffectPrefab, hitColl.point, Quaternion.LookRotation(hitColl.normal));
            LeanPool.Despawn(clone, 1f);
            DespawnBullet();
        }
    }
    private void OnEnable()
    {
        Invoke("DespawnBullet",3f);
        
        //rb.AddForce(transform.forward * speed,ForceMode.Impulse);
    }
    public void DespawnBullet()
    {
        SpawnManager.Instance.Despawn(gameObject);
    }
}