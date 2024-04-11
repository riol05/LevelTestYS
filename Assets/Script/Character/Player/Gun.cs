using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Projectile bulletPrefab;

    public Transform gunPivot;

    public void SpawnBullet()
    {
        SpawnManager.Instance.BulletSpawn(bulletPrefab, gunPivot.forward, gunPivot.rotation, transform);
    }
    
}
