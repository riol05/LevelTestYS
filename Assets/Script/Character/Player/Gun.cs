using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Projectile bulletPrefab;

    public Transform gunPivot;

    public void SpawnBullet()
    {
        // 총알의 위치와 방향 설정
        Vector3 bulletPosition = gunPivot.position;
        Quaternion bulletRotation = gunPivot.rotation;

        // 총알 생성
        Instantiate(bulletPrefab, bulletPosition, bulletRotation);
    }

}
