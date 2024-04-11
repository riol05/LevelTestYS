using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean;
using UnityEngine.Pool;
using Lean.Pool;
using Unity.VisualScripting;
using System;
using TreeEditor;
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    public GameObject MonsterPrefab;
    float spawnInterval;
    public List<Transform> spawnTransform;

    public Coroutine spawnRoutine = null;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Time.time > spawnInterval + 2)
        {
            int i = UnityEngine.Random.Range(0, spawnTransform.Count);
            SpawnMonster(MonsterPrefab, spawnTransform[i].position, spawnTransform[i]);
        }

    }
    public void SpawnMonster(GameObject monster, Vector3 dir, Transform parent)
    {
         LeanPool.Spawn(monster, dir, Quaternion.identity, parent);
         spawnInterval = Time.time;
    }
    public void Despawn(GameObject obj)
    {
        LeanPool.Despawn(obj);
    }
    
    public void BulletSpawn(Projectile bullet,Vector3 dir, Quaternion rot,Transform parent)
    {
        LeanPool.Spawn(bullet,dir,rot,parent);
        bullet.rb.AddForce(dir * bullet.speed,ForceMode.Impulse);
    }
}
