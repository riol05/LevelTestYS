using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Projectile bulletPrefab;

    public Transform gunPivot;

    public void SpawnBullet()
    {
        // �Ѿ��� ��ġ�� ���� ����
        Vector3 bulletPosition = gunPivot.position;
        Quaternion bulletRotation = gunPivot.rotation;

        // �Ѿ� ����
        Instantiate(bulletPrefab, bulletPosition, bulletRotation);
    }

}
